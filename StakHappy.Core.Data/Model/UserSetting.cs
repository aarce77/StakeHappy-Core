using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StakHappy.Core.Data.Model
{
    public partial class UserSetting : IEntity
    {
        public Guid Id { get; set; }
        [ForeignKey("User")]
        [Required]
        public Guid User_Id { get; set; }
        [MaxLength(300)]
        public String Code { get; set; }
        [MaxLength(300)]
        public String Name { get; set; }
        [MaxLength(500)]
        public String ObjectFullName { get; set; }
        public String XmlData { get; set; }
        public String JsonData { get; set; }
        public int DataType { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public DateTime CreatedData { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        public virtual User User { get; set; }
    }
}
