using System;
using System.Linq;

namespace StakHappy.Core.Logic
{
    public class UserServiceLogic : LogicBase<Data.Model.UserService>
    {
        #region Constructor
        public UserServiceLogic()
        {
            Persistor = Dependency.Get<Data.Persistor.UserService>();
        }

        public UserServiceLogic(Data.Persistor.UserService userServicePersistor)
        {
            Persistor = userServicePersistor;
        }
        #endregion

        /// <summary>
        /// Saves the specified user service.
        /// </summary>
        /// <param name="userService">The user service.</param>
        [TransactionInterceptor]
        public override Data.Model.UserService Save(Data.Model.UserService userService)
        {
            if(userService.User_Id == Guid.Empty)
                throw new ArgumentException("user id most be specified to save a user service");

            return base.Save(userService);
        }

        public virtual IQueryable<Data.Model.UserService> GetList(
            System.Linq.Expressions.Expression<Func<Data.Model.UserService, bool>> predicate)
        {
            return Persistor.Get(predicate);
        }
    }

    
}