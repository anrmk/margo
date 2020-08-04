using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace Core.Data.Entities {
  //  [Table(name: "ApplicationUsers")]
    public class AspNetUserEntity: IdentityUser {
        [ForeignKey("Profile")]
        [Column("ProfileId")]
        public long? Profile_Id { get; set; }
        public virtual AspNetUserProfileEntity Profile { get; set; }
    }
}
