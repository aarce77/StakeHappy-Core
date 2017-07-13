using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StakHappy.Core.Data.Model
{
    public partial class UserService : IEntity
    {
        public Guid Id { get; set; }
        [ForeignKey("User")]
        [Required]
        public Guid User_Id { get; set; }
        [MaxLength(300)]
        public String Label { get; set; }
        public String Description { get; set; }
        public long Price { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime LastModified { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        public virtual User User { get; set; }
    }
}
