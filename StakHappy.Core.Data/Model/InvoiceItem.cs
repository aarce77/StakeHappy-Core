using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StakHappy.Core.Data.Model
{
    public partial class InvoiceItem : IEntity
    {
        public Guid Id { get; set; }

        [ForeignKey("Invoice")]
        [Required]
        public Guid Invoice_Id { get; set; }

        [ForeignKey("Service")]
        public Guid? Service_Id { get; set; }

        [Required]
        public int Quantity { get; set; }
        [Required]
        public long UnitCost { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public long? DiscountAmount { get; set; }
        public String Description { get; set; }
        public DateTime? DeletedDate { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime LastModified { get; set; }

        private Currency.Currency _currency;
        [NotMapped]
        public Currency.Currency CurrencySubTotal
        {
            get
            {
                if(_currency == null)
                    _currency = new Currency.Currency(SubTotal);
                else
                    _currency.SetCurrency(SubTotal);
                
                return _currency;
            }
        }

        [NotMapped]
        public long SubTotal
        {
            get { return Quantity*UnitCost; }
        }

        [System.Runtime.Serialization.IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        public virtual Invoice Invoice { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        public virtual UserService Service { get; set; }
    }
}
