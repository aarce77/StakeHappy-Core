using System;
using System.Linq;

namespace StakHappy.Core.Data.Persistor
{
    public class Client : BasePersistor<Model.Client>
    {
        internal virtual IQueryable<Model.Client> Search(Search.ClientCriteria critiera)
        {
            var result = Repository.FindAll();
            result = critiera.SortOrder == SortDirection.ASC ? result.OrderBy(o => o.CompanyName) :
                result.OrderByDescending(o => o.CompanyName);

            if (critiera.UserId != Guid.Empty)
                result = result.Where(r => r.User.Id == critiera.UserId);
            if (!string.IsNullOrEmpty(critiera.CompanyName))
                result = result.Where(r => r.CompanyName.Contains(critiera.CompanyName));

            if (critiera.Page.HasValue && critiera.PageSize.HasValue)
                result = result.Skip(critiera.Skip).Take(critiera.PageSize.Value);   

            return result;
        }
    }
}
