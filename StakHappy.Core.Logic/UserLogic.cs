using System;
using System.Linq;
using StakHappy.Core.Data.Model;

namespace StakHappy.Core.Logic
{
    public class UserLogic : LogicBase<Data.Model.User>
    {
        #region Dependencies
        private readonly Validation.IValidator Validator;
        #endregion

        #region Constructor
        public UserLogic() {
            Persistor = Dependency.Get<Data.Persistor.User>();
            Validator = Dependency.Get<Validation.UserValidator>();
        }

        public UserLogic(Data.Persistor.User persistor, Validation.IValidator validator) {
            Persistor = persistor;
            Validator = validator;
        }
        #endregion

        [TransactionInterceptor]
        public override Data.Model.User Save(Data.Model.User user)
        {
            var result = Validator.Validate(user);

            if (result.IsValid)
            {
                var entity = Persistor.Save(user);
                Persistor.Commit();
                return entity;
            }
            var properties = result.Errors.Select(e => e.PropertyName).ToArray();
            throw new ArgumentException("One or more properties a required",
                String.Join(", ", properties.ToArray()));
        }

        public override User GetNewModelIntance()
        {
            var user = base.GetNewModelIntance();
            user.Active = true;
            user.CreatedDate = DateTime.Now;
            return user;
        }

        /// <summary>
        /// Determines whether the username us in use by a different user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">username is required</exception>
        public virtual bool IsUserNameInUse(string username, Guid userId = default(Guid))
        {
            if(string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");

            return (Persistor as Data.Persistor.User).IsUserNameInUse(username, userId);
        }

        /// <summary>
        /// Gets the by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public virtual Data.Model.User GetByUsername(string username)
        {
            return Persistor.Get(u => u.UserName == username).FirstOrDefault();
        }

        /// <summary>
        /// Deactivates a user by the specified id.
        /// </summary>
        /// <param name="id">The user id.</param>
        public virtual void Deactivate(Guid id)
        {
            var user = Persistor.Get(id);

            user.Active = false;
            Persistor.Save(user);
            Persistor.Commit();
        }

    }
}
