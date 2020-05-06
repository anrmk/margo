using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class VaccountFilterViewModel: PagerFilterViewModel {
        [Display(Name = "Company")]
        public long? CompanyId { get; set; }

        [Display(Name = "Vendor")]
        public long? VendorId { get; set; }
    }
}
