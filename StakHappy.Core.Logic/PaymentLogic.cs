using System;
using System.Collections.Generic;
using System.Linq;

namespace StakHappy.Core.Logic
{
    public class PaymentLogic : LogicBase
    {
        #region Dependencies
        private Data.Persistor.Payment _paymentPersistor;
        internal Data.Persistor.Payment PaymentPersistor
        {
            get { return Dependency.Get(_paymentPersistor); }
            set { _paymentPersistor = value; }
        }
        #endregion

        /// <summary>
        /// Reads a payment by the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual Data.Model.Payment Get(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Payment id cannot be empty");
            return PaymentPersistor.Get(id);
        }

        public virtual IQueryable<Data.Model.Payment> GetPayments(Guid invoiceId)
        {
            return PaymentPersistor.Get(p => p.Invoice_Id == invoiceId);
        }

        /// <summary>
        /// Saves the payments.s
        /// </summary>
        /// <param name="payment">The payment.</param>
        [TransactionInterceptor]
        public virtual Data.Model.Payment Save(Data.Model.Payment payment)
        {
            if (payment.Invoice_Id == Guid.Empty)
                throw new ArgumentException("Invoice id most be specified to apply a payment");

            var entity = PaymentPersistor.Save(payment);
            PaymentPersistor.Commit();

            return entity;
        }

        /// <summary>
        /// Deletes the payment by the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="System.ArgumentException">client id cannot be empty</exception>
        [TransactionInterceptor]
        public virtual void Delete(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Payment id cannot be empty");

            PaymentPersistor.Delete(id);
            PaymentPersistor.Commit();
        }

        public virtual Data.Model.Payment GetNewPaymentObject()
        {
            return PaymentPersistor.Create();
        }

        public virtual List<Data.Model.PaymentType> GetPaymentTypes()
        {
            return new List<Data.Model.PaymentType>
            {
                new Data.Model.PaymentType {Id = 1, Code = "Cash", Label = "Cash", Description = "Cash payment"},
                new Data.Model.PaymentType {Id = 2, Code = "Check", Label = "Check", Description = "Check payment"},
                new Data.Model.PaymentType
                {
                    Id = 3,
                    Code = "Credit",
                    Label = "Credit",
                    Description = "Credit card payment"
                },
                new Data.Model.PaymentType
                {
                    Id = 4,
                    Code = "Online",
                    Label = "Online",
                    Description = "Online payment such as PayPal, Square, etc"
                }
            };
        }
    }
}