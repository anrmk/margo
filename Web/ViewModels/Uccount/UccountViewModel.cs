using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Core.Data.Enums;

namespace Web.ViewModels {
    public class UccountViewModel {
        public long Id { get; set; }
        public string Name { get; set; }
        [Required]
        public UccountTypes Kind { get; set; }

        [Display(Name = "Company")]
        public long? CompanyId { get; set; }
        [Required]
        [Display(Name = "Vendor")]
        public long VendorId { get; set; }
        public string VendorName { get; set; }
        [Display(Name = "Person")]
        public long? PersonId { get; set; }

        public IEnumerable<UccountServiceViewModel> Services { get; set; }
        public IEnumerable<UccountVendorFieldViewModel> Fields { get; set; }
    }
}
