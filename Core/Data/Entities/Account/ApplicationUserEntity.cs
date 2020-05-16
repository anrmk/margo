using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace Core.Data.Entities {
    [Table(name: "ApplicationUsers")]
    public class ApplicationUserEntity: IdentityUser {
        [ForeignKey("Profile")]
        public long? Profile_Id { get; set; }
        public virtual ApplicationUserProfileEntity Profile { get; set; }
    }
}
