using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace Core.Data.Entities {
    [Table(name: "AspNetUsers")]
    public class AspNetUserEntity: IdentityUser {
        public virtual AspNetUserProfileEntity Profile { get; set; }

        public virtual ICollection<AspNetUserDenyAccessCompanyEntity> CompanyGrants { get; set; }
        public virtual ICollection<AspNetUserDenyAccessCategoryEntity> CategoryGrants { get; set; }

        public virtual ICollection<AspNetUserCompanyFavouriteEntity> FavouriteCompanies { get; set; }
    }
}
