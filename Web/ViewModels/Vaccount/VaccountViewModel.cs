using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class VaccountViewModel {
        public long Id { get; set; }

        [Required]
        [MaxLength(32)]
        [Display(Name = "Username/Email")]
        public string UserName { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public long? CompanyId { get; set; }
        public CompanyViewModel Company { get; set; }

        public long? VendorId { get; set; }
        public VendorViewModel Vendor { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
