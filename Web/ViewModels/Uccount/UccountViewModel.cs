using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class UccountViewModel {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Kind { get; set; }

        [Display(Name = "Company")]
        public long? CompanyId { get; set; }
        [Display(Name = "Vendor")]
        public long? VendorId { get; set; }
        [Display(Name = "Person")]
        public long? PersonId { get; set; }

        public IEnumerable<UccountServiceViewModel> Services { get; set; }
        public IEnumerable<UccountVendorFieldViewModel> VendorFields { get; set; }
    }
}
