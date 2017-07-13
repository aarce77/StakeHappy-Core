using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StakHappy.Core.Data.Model
{
    public partial class ClientContact : IEntity
    {
        public Guid Id { get; set; }
        [ForeignKey("Client")]
        [Required]
        public Guid Client_Id { get; set; }
        [MaxLength(150)]
        public String FirstName { get; set; }
        [MaxLength(150)]
        public String LastName { get; set; }
        [MaxLength(300)]
        public String Email { get; set; }
        [MaxLength(30)]
        public String PrimaryPhone { get; set; }
        [MaxLength(30)]
        public String SecondaryPhone { get; set; }
        [Required]
        public bool IsPrimary { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        public virtual Client Client { get; set; }
    }
}
