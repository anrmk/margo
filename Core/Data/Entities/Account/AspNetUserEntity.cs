using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace Core.Data.Entities {
    [Table(name: "AspNetUsers")]
    public class AspNetUserEntity: IdentityUser {
        [ForeignKey("Profile")]
        [Column("ProfileId")]
        public long? Profile_Id { get; set; }
        public virtual AspNetUserProfileEntity Profile { get; set; }

        public virtual ICollection<AspNetUserGrantEntity> Grants { get; set; }
    }
}
