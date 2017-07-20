using System;
using StakHappy.Core.Data.Model;

namespace StakHappy.Core.Logic
{
    public abstract class LogicBase<T> where T : class, IEntity, new()
    {
        internal readonly DependencyResolver Dependency;
        internal Data.Persistor.BasePersistor<T> Persistor;

        #region Constructor
        /// <summary>
        /// Initializes the <see cref="LogicBase"/> class.
        /// </summary>
        protected LogicBase()
        {
            Dependency = new DependencyResolver();
        }
        #endregion

        public virtual T Get(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id cannot be empty");
            return Persistor.Get(id);
        }

        public virtual System.Linq.IQueryable<T> GetAll()
        {
            return Persistor.Get();
        }

        public virtual System.Linq.IQueryable<T> Find(
            System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Persistor.Get(predicate);
        }

        [TransactionInterceptor]
        public virtual T Save(T entity)
        {
            var instance = Persistor.Save(entity);
            Persistor.Commit();

            return instance;
        }

        [TransactionInterceptor]
        public virtual bool Delete(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id cannot be empty");

            Persistor.Delete(id);
            return Persistor.Commit() > 0;
        }

        /// <summary>
        /// Gets a new instance of model object
        /// </summary>
        /// <returns></returns>
        public virtual T GetNewModelIntance()
        {
            return new T();
        }

        /// <summary>
        /// Vaildates the criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <exception cref="System.ArgumentNullException">
        /// critiera;Search critiera is required
        /// or
        /// UserId;client search cannot be conducted without specifying the user
        /// </exception>
        /// <remarks>This method is tested in the client/SearchFixture unit test</remarks>
        internal virtual void VaildateCriteria(Data.Search.SearchCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException("critiera", "Search critiera is required");
            if (criteria.UserId == Guid.Empty)
                throw new ArgumentNullException("UserId",
                    "client search cannot be conducted without specifying the user");
        }
    }
}
