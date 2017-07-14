using System;
using System.Linq;

namespace StakHappy.Core.Logic
{
    public class InvoiceLogic : LogicBase
    {
        #region Dependencies
        private Data.Persistor.Invoice _invoicePersistor;
        internal Data.Persistor.Invoice InvoicePersistor
        {
            get { return Dependency.Get(_invoicePersistor); }
            set { _invoicePersistor = value; }
        }

        private Data.Persistor.InvoiceItem _invoiceItemPersistor;
        internal Data.Persistor.InvoiceItem InvoiceItemPersistor
        {
            get { return Dependency.Get(_invoiceItemPersistor); }
            set { _invoiceItemPersistor = value; }
        }
        #endregion

        /// <summary>
        /// Reads the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual Data.Model.Invoice Get(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("invoice id cannot be empty");
            return InvoicePersistor.Get(id);
        }

        /// <summary>
        /// searches the invoices.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public virtual IQueryable<Data.Model.Invoice> Search(Data.Search.InvoiceCriteria criteria)
        {
            VaildateCriteria(criteria);
            return InvoicePersistor.Search(criteria);
        }

        /// <summary>
        /// Vaildates the criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <exception cref="NullReferenceException">Invoice search criteria could not be cast to the proper type</exception>
        /// <remarks>
        /// This method is tested in the client/SearchFixture unit test
        /// </remarks>
        internal override void VaildateCriteria(Data.Search.SearchCriteria criteria)
        {
            base.VaildateCriteria(criteria);

            var invCriteria = (criteria as Data.Search.InvoiceCriteria);
            if(invCriteria == null)
                throw new NullReferenceException("Invoice search criteria could not be cast to the proper type");

            if (invCriteria.VoidedDateRange != null &&
                (invCriteria.VoidedDateRange.From != default(DateTime) ||
                 invCriteria.VoidedDateRange.To != default(DateTime)))
            {
                invCriteria.Voided = true;
            }
        }

        /// <summary>
        /// Saves the specified client.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="invoice">The client.</param>
        [TransactionInterceptor]
        public virtual Data.Model.Invoice Save(Guid userId, Data.Model.Invoice invoice)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User id most be specified to save an invoice");

            if(invoice.Client_Id == Guid.Empty)
                throw new ArgumentException("Client id most be specified to save an invoice");

            var isNew = invoice.Id == Guid.Empty;
            var result = InvoicePersistor.Save(invoice);

            if (!isNew)
            {
                InvoicePersistor.Commit();
                InvoicePersistor.UpdateUserId(userId, invoice.Id);
                return result;
            }

            foreach (var item in invoice.Items)
            {
                item.Invoice_Id = result.Id;
                InvoiceItemPersistor.Save(item);
            }

            InvoicePersistor.Commit();
            if (invoice.Items.Any())
                InvoiceItemPersistor.Commit();
            InvoicePersistor.UpdateUserId(userId, invoice.Id);

            return result;
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
                throw new ArgumentException("invoice id cannot be empty");

            var invoice = InvoicePersistor.Get(id);
            // verify that the invoice dosen't have any payments
            // we can't delete invoices that have payments
            if (invoice.Payments != null && invoice.Payments.Count > 0)
                throw new NotImplementedException("an invoice that contains a payment can not be deleted");

            InvoicePersistor.Delete(id);
            InvoicePersistor.Commit();
        }

        /// <summary>
        /// Gets the contants.
        /// </summary>
        /// <param name="invoiceItem">The client contact.</param>
        /// <returns></returns>
        [TransactionInterceptor]
        public virtual Data.Model.InvoiceItem SaveInvoiceItem(Data.Model.InvoiceItem invoiceItem)
        {
            if (invoiceItem.Invoice_Id == Guid.Empty)
                throw new ArgumentException("invoice id cannot be empty");

            var item = InvoiceItemPersistor.Save(invoiceItem);
            InvoiceItemPersistor.Commit();

            return item;
        }

        /// <summary>
        /// Deletes the contact.
        /// </summary>
        /// <param name="id">The client contact identifier.</param>
        [TransactionInterceptor]
        public virtual void DeleteInvoiceItem(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("invoice item id cannot be empty");
            InvoiceItemPersistor.Delete(id);
            InvoiceItemPersistor.Commit();
        }

        public virtual Data.Model.Invoice GetNewInvoiceObject()
        {
            return InvoicePersistor.Create();
        }

        public virtual Data.Model.InvoiceItem GetNewInvoiceItemObject()
        {
            return InvoiceItemPersistor.Create();
        }
    }
}
