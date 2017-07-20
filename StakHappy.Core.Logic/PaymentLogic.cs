using System;
using System.Collections.Generic;
using System.Linq;

namespace StakHappy.Core.Logic
{
    public class PaymentLogic : LogicBase<Data.Model.Payment>
    {
        #region Constructor
        public PaymentLogic()
        {
            Persistor = Dependency.Get<Data.Persistor.Payment>();
        }
        public PaymentLogic(Data.Persistor.Payment paymentPersistor)
        {
            Persistor = paymentPersistor;
        }
        #endregion

        /// <summary>
        /// Saves the payments.s
        /// </summary>
        /// <param name="payment">The payment.</param>
        [TransactionInterceptor]
        public override Data.Model.Payment Save(Data.Model.Payment payment)
        {
            if (payment.Invoice_Id == Guid.Empty)
                throw new ArgumentException("Invoice id most be specified to apply a payment");

            return base.Save(payment);
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