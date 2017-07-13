using System;
using System.Linq;

namespace StakHappy.Core.Data.Persistor
{
    public class Invoice : BasePersistor<Model.Invoice>
    {
        /// <summary>
        /// Searches the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        internal virtual IQueryable<Model.Invoice> Search(Search.InvoiceCriteria criteria)
        {
            var results = Repository.FindAll().Where(r => r.Client.User.Id == criteria.UserId);

            if (criteria.HasBalanceOnly)
                results = results.Where(r => r.Items.Sum(i => i.Quantity * i.UnitCost) > r.Payments.Sum(p => p.Amount));

            if (criteria.ClientId != default(Guid))
                results = results.Where(r => r.Client.Id == criteria.ClientId);

            if (!String.IsNullOrEmpty(criteria.Number))
                results = results.Where(r => r.Number.Contains(criteria.Number));

            if (criteria.InvoiceDateRange != null)
            {
                if (criteria.InvoiceDateRange.From != default(DateTime))
                    results = results.Where(r => r.Date >= criteria.InvoiceDateRange.From);
                if (criteria.InvoiceDateRange.To != default(DateTime))
                    results = results.Where(r => r.Date <= criteria.InvoiceDateRange.To);
            }

            results = criteria.Voided.HasValue ? results.Where(r => r.Voided == criteria.Voided.Value) : 
                results.Where(r => r.Voided == null || r.Voided == false);

            if (criteria.VoidedDateRange != null)
            {
                if (criteria.VoidedDateRange.From != default(DateTime))
                    results = results.Where(r => r.VoidedDate >= criteria.VoidedDateRange.From);
                if (criteria.VoidedDateRange.To != default(DateTime))
                    results = results.Where(r => r.VoidedDate <= criteria.VoidedDateRange.To);
            }

            switch (criteria.SortBy)
            {
                case Data.Search.InvoiceCriteria.SortFields.Client:
                    results = criteria.SortOrder == SortDirection.ASC
                        ? results.OrderBy(r => r.Client.CompanyName)
                        : results.OrderByDescending(r => r.Client.CompanyName);
                    break;
                case Data.Search.InvoiceCriteria.SortFields.Date:
                    results = criteria.SortOrder == SortDirection.ASC
                        ? results.OrderBy(r => r.Date)
                        : results.OrderByDescending(r => r.Date);
                    break;
                default:
                    results = criteria.SortOrder == SortDirection.ASC
                        ? results.OrderBy(r => r.Number)
                        : results.OrderByDescending(r => r.Number);
                    break;
            }

            if (criteria.Page.HasValue && criteria.PageSize.HasValue && criteria.Page.Value > 0)
                results = results.Skip(criteria.Skip).Take(criteria.PageSize.Value);

            return results;
        }

        /// <summary>
        /// Updates the user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        internal virtual void UpdateUserId(Guid userId, Guid invoiceId)
        {
            var sql = string.Format("UPDATE Invoices SET User_Id = '{0}' WHERE Id = '{1}'", userId, invoiceId);
            DbContext.Database.ExecuteSqlCommand(sql);
        }
    }
}
