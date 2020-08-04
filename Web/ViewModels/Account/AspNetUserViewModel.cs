using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels {
    public class AspNetUserViewModel {
        public string Id { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "Normalized User Name")]
        public string NormalizedUserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Email confirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Phone number confirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [Required]
        [Display(Name = "Roles")]
        public List<string> Roles { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        [Display(Name = "Profile")]
        public long? ProfileId { get; set; }

        public virtual AspNetUserProfileViewModel Profile { get; set; }
    }
}
