using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "AspNetUserProfiles")]
    public class AspNetUserProfileEntity: AuditableEntity<long> {
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(64)]
        public string SurName { get; set; }

        [MaxLength(64)]
        public string MiddleName { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [ForeignKey("User")]
        [Column("User_Id")]
        public string UserId { get; set; }
        public virtual AspNetUserEntity User { get; set; }
    }
}
