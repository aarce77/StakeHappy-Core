using System;
using System.Linq;
using StakHappy.Core.Data.Model;

namespace StakHappy.Core.Data.Persistor
{
    public abstract class BasePersistor<T> where T : class, IEntity, new()
    {
        internal Context DbContext { get; set; }
        internal readonly SqlRepository<T> Repository;

        #region Constructor
        protected BasePersistor()
        {
            var connectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["StakHappy"].ConnectionString;
            DbContext = new Context(connectionString);
            Repository = new SqlRepository<T>(DbContext);
        }
        #endregion

        #region Methods
        internal virtual T Get(Guid id)
        {
            return Repository.Find(e => e.Id == id).SingleOrDefault();
        }

        internal virtual IQueryable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Repository.Find(predicate);
        }

        internal virtual IQueryable<T> Get()
        {
            return Repository.FindAll();
        }

        internal virtual T Save(T entity)
        {
            return Repository.Save(entity);
        }

        internal virtual void Delete(Guid id)
        {
            var entity = Repository.Find(e => e.Id == id).SingleOrDefault();
            if(entity != null)
                Repository.Delete(entity);
        }

        internal virtual T Create()
        {
            return new T();
        }

        internal virtual int Commit()
        {
            return Repository.Commit();
        }

        internal virtual void Dispose()
        {
            Repository.Dispose();
        }
        #endregion
    }
}
