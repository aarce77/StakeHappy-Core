using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StakHappy.Core.Data.Model
{
    public partial class User : IEntity
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(150)]
        [Index("IX_User_UserName", IsUnique = true, Order = 1)]
        public String UserName { get; set; }
        [MaxLength(150)]
        public String FirstName { get; set; }
        [MaxLength(150)]
        public String LastName { get; set; }
        [MaxLength(100)]
        public String CompanyName { get; set; }
        [MaxLength(100)]
        public String DisplayName { get; set; }
        [Required]
        [MaxLength(300)]
        [EmailAddress]
        [Index("IX_User_Email",IsUnique=true, Order=1)]
        public String Email { get; set; }
        [MaxLength(30)]
        public String PrimaryPhone { get; set; }
        [MaxLength(30)]
        public String SecondaryPhone { get; set; }
        [MaxLength(300)]
        public String AddressLine1 { get; set; }
        [MaxLength(300)]
        public String AddressLine2 { get; set; }
        [MaxLength(300)]
        public String City { get; set; }
        [MaxLength(150)]
        public String State { get; set; }
        [MaxLength(30)]
        public String PostalCode { get; set; }
        [MaxLength(150)]
        public String Country { get; set; }
        public Boolean Active { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<Client> Clients { get; set; }
        public List<Invoice> Invoices { get; set; }

        public List<UserService> Services { get; set; }
        public List<UserSetting> Settings { get; set; }
    }
}
