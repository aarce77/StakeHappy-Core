using System;
using System.Linq;

namespace StakHappy.Core.Data.Persistor
{
    public class User : BasePersistor<Model.User>
    {
        /// <summary>
        /// Determines whether the specified username is in use.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="userId">If provided, executes the user from the search.</param>
        public virtual bool IsUserNameInUse(string username, Guid userId = default(Guid))
        {
            var results = Repository.FindAll().Where(u => u.UserName == username);
            if (userId != default(Guid))
                results = results.Where(u => u.Id != userId);

            return results.FirstOrDefault() != null;
        }
    }
}
