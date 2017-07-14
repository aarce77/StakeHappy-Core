using System;
using StakHappy.Core.Data.Model;

namespace StakHappy.Core.Logic
{
    public abstract class LogicBase
    {
        internal readonly DependencyResolver Dependency;
        /// <summary>
        /// Initializes the <see cref="LogicBase"/> class.
        /// </summary>
        protected LogicBase() 
        {
            Dependency = new DependencyResolver();
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
