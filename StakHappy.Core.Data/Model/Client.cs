using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StakHappy.Core.Data.Model
{
    public partial class Client : IEntity
    {
        public Guid Id { get; set; }
        [ForeignKey("User")]
        [Required]
        public Guid User_Id { get; set; }
        [MaxLength(30)]
        public String BusinessPhone { get; set; }
        [MaxLength(300)]
        public String AddressLine1 { get; set; }
        [MaxLength(300)]
        public String AddressLine2 { get; set; }
        [MaxLength(300)]
        public String City { get; set; }
        [MaxLength(150)]
        public String State { get; set; }
        [MaxLength(20)]
        public String PostalCode { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [MaxLength(300)]
        public String CompanyName { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        public virtual User User { get; set; }
        public List<ClientContact> Contacts { get; set; }
        public List<Invoice> Invoices { get; set; }
    }
}
