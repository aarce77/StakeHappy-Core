using System;
using System.Linq;
using System.Linq.Expressions;

namespace StakHappy.Core.Logic
{
    public class InvoiceLogic : LogicBase<Data.Model.Invoice>
    {
        #region Dependencies
        private readonly Data.Persistor.InvoiceItem InvoiceItemPersistor;
        private readonly Data.Persistor.Client ClientPersistor;
        #endregion

        #region Constructor
        public InvoiceLogic()
        {
            Persistor = Dependency.Get<Data.Persistor.Invoice>();
            InvoiceItemPersistor = Dependency.Get<Data.Persistor.InvoiceItem>();
            ClientPersistor = Dependency.Get<Data.Persistor.Client>();
        }

        public InvoiceLogic(
            Data.Persistor.Invoice invoicePersistor, 
            Data.Persistor.InvoiceItem invoiceItemPersistor,
            Data.Persistor.Client clientPersistor)
        {
            Persistor = invoicePersistor;
            InvoiceItemPersistor = invoiceItemPersistor;
            ClientPersistor = clientPersistor;
        }
        #endregion

        public override Data.Model.Invoice Save(Data.Model.Invoice invoice)
        {
            if (invoice.Client_Id == Guid.Empty)
                throw new ArgumentException("Client id most be specified to save an invoice");

            var userId = ClientPersistor.Get(invoice.Client_Id).User_Id;

            return Save(userId, invoice);
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
            var result = Persistor.Save(invoice);

            if (!isNew)
            {
                Persistor.Commit();
                (Persistor as Data.Persistor.Invoice).UpdateUserId(userId, invoice.Id);
                return result;
            }

            foreach (var item in invoice.Items)
            {
                item.Invoice_Id = result.Id;
                InvoiceItemPersistor.Save(item);
            }

            Persistor.Commit();
            if (invoice.Items.Any())
                InvoiceItemPersistor.Commit();
            (Persistor as Data.Persistor.Invoice).UpdateUserId(userId, invoice.Id);

            return result;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="System.ArgumentException">client id cannot be empty</exception>
        [TransactionInterceptor]
        public override bool Delete(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("invoice id cannot be empty");

            var invoice = Persistor.Get(id);
            // verify that the invoice dosen't have any payments
            // we can't delete invoices that have payments
            if (invoice.Payments != null && invoice.Payments.Count > 0)
                throw new NotImplementedException("an invoice that contains a payment can not be deleted");

            Persistor.Delete(id);
            return Persistor.Commit() > 0;
        }

        /// <summary>
        /// searches the invoices.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public virtual IQueryable<Data.Model.Invoice> Search(Data.Search.InvoiceCriteria criteria)
        {
            VaildateCriteria(criteria);
            return (Persistor as Data.Persistor.Invoice).Search(criteria);
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
            if (invCriteria == null)
                throw new NullReferenceException("Invoice search criteria could not be cast to the proper type");

            if (invCriteria.VoidedDateRange != null &&
                (invCriteria.VoidedDateRange.From != default(DateTime) ||
                 invCriteria.VoidedDateRange.To != default(DateTime)))
            {
                invCriteria.Voided = true;
            }
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
        public virtual bool DeleteInvoiceItem(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("invoice item id cannot be empty");
            InvoiceItemPersistor.Delete(id);
            InvoiceItemPersistor.Commit();

            return true;
        }

        public virtual Data.Model.InvoiceItem GetNewInvoiceItemModel()
        {
            return InvoiceItemPersistor.Create();
        }
    }
}
