using System;
using System.Linq;

namespace StakHappy.Core.Logic
{
    public class UserServiceLogic : LogicBase
    {
        #region Dependencies
        private Data.Persistor.UserService _userServicePersistor;
        internal Data.Persistor.UserService UserServicePersistor
        {
            get { return Dependency.Get(_userServicePersistor); }
            set { _userServicePersistor = value; }
        }
        #endregion

        public virtual Data.Model.UserService Get(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("service id cannot be empty");
            return UserServicePersistor.Get(id);
        }

        public virtual IQueryable<Data.Model.UserService> GetList(System.Linq.Expressions.Expression<Func<Data.Model.UserService, bool>> predicate)
        {
            return UserServicePersistor.Get(predicate);
        }

        /// <summary>
        /// Saves the specified user service.
        /// </summary>
        /// <param name="userService">The user service.</param>
        [TransactionInterceptor]
        public virtual Data.Model.UserService Save(Data.Model.UserService userService)
        {
            if(userService.User_Id == Guid.Empty)
                throw new ArgumentException("user id most be specified to save a user service");

            var service = UserServicePersistor.Save(userService);
            UserServicePersistor.Commit();
            return service;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="System.ArgumentException">client id cannot be empty</exception>
        [TransactionInterceptor]
        public virtual void Delete(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("user service id cannot be empty");

            UserServicePersistor.Delete(id);
            UserServicePersistor.Commit();
        }
    }

    
}