using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StakHappy.Core.Data.Model
{
    public partial class Payment : IEntity
    {
        public Guid Id { get; set; }
        [ForeignKey("Invoice")]
        [Required]
        public Guid Invoice_Id { get; set; }
        [Required]
        public long Amount { get; set; }
        public String Description { get; set; }
        public int? TypeId { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime LastModified { get; set; }
        [Required]
        public bool Active { get; set; }
        public DateTime? VoidedDate { get; set; }
        [MaxLength(300)]
        public String Reference { get; set; }

        private Currency.Currency _currency;
        [NotMapped]
        public Currency.Currency Currency
        {
            get
            {
                if (_currency == null)
                    _currency = new Currency.Currency(Amount);
                else
                    _currency.SetCurrency(Amount);

                return _currency;
            }
        }

        [System.Runtime.Serialization.IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        public virtual Invoice Invoice { get; set; }
    }
}
