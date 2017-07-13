using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace StakHappy.Core.Data.Model
{
    public partial class Invoice : IEntity
    {
        public Guid Id { get; set; }

        [ForeignKey("Client")]
        [Required]
        public Guid Client_Id { get; set; }

        [MaxLength(100)]
        public String Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime? PaymentRequiredDate { get; set; }
        public int? PaymentTerm { get; set; }
        public bool? Voided { get; set; }
        public DateTime? VoidedDate { get; set; }
        [Required]
        public bool Active { get; set; }
        public bool? PaidInFull { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime LastModified { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        public virtual Client Client { get; set; }
        public List<InvoiceItem> Items { get; set; }
        public List<Payment> Payments { get; set; }

        private Currency.Currency _total;
        [NotMapped]
        public Currency.Currency Total
        {
            get
            {
                if (_total == null)
                    _total = new Currency.Currency(Items.Sum(i => i.SubTotal));
                else
                    _total.SetCurrency(Items.Sum(i => i.SubTotal));

                return _total;
            }
        }

        private Currency.Currency _totalPayments;
        [NotMapped]
        public Currency.Currency TotalPayments
        {
            get
            {
                if (_totalPayments == null)
                    _totalPayments = new Currency.Currency(Payments.Sum(p => p.Amount));
                else
                    _totalPayments.SetCurrency(Payments.Sum(p => p.Amount));

                return _totalPayments;
            }
        }
    }
}
