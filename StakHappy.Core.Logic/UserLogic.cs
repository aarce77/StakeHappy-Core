using System;
using System.Linq;

namespace StakHappy.Core.Logic
{
    public class UserLogic : LogicBase
    {
        #region Dependencies
        private Data.Persistor.User _persistor;
        private Validation.IValidator _validator;
        #endregion

        #region Constructor
        public UserLogic() {
            _persistor = Dependency.Get<Data.Persistor.User>();
            _validator = Dependency.Get<Validation.UserValidator>();
        }

        public UserLogic(Data.Persistor.User persistor, Validation.IValidator validator) {
            _persistor = persistor;
            _validator = validator;
        }
        #endregion

        /// <summary>
        /// Retrieves a user by the specified id.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <returns></returns>
        public virtual Data.Model.User Get(Guid id)
        {
            if(id == Guid.Empty)
                throw new ArgumentException("user id cannot be empty");

            return _persistor.Get(id);
        }

        /// <summary>
        /// Saves the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        [TransactionInterceptor]
        public virtual Data.Model.User Save(Data.Model.User user)
        {
            var result = _validator.Validate(user);

            if (result.IsValid)
            {
                var entity = _persistor.Save(user);
                _persistor.Commit();
                return entity;
            }
            var properties = result.Errors.Select(e => e.PropertyName).ToArray();
            throw new ArgumentException("One or more properties a required",
                String.Join(", ", properties.ToArray()));
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

            return _persistor.IsUserNameInUse(username, userId);
        }

        /// <summary>
        /// Gets the by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public virtual Data.Model.User GetByUsername(string username)
        {
            return _persistor.Get(u => u.UserName == username).FirstOrDefault();
        }

        /// <summary>
        /// Deactivates a user by the specified id.
        /// </summary>
        /// <param name="id">The user id.</param>
        public virtual void Deactivate(Guid id)
        {
            var user = _persistor.Get(id);

            user.Active = false;
            _persistor.Save(user);
        }

        /// <summary>
        /// Gets a new user model object
        /// </summary>
        /// <returns></returns>
        public virtual Data.Model.User GetNewUser()
        {
            return _persistor.Create();
        }

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual IQueryable<Data.Model.User> Find(
            System.Linq.Expressions.Expression<Func<Data.Model.User, bool>> predicate)
        {
            return _persistor.Get(predicate);
        }
    }
}
