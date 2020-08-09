using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class VaccountFilterViewModel: PagerFilterViewModel {
        [Display(Name = "Company")]
        public Guid? CompanyId { get; set; }

        [Display(Name = "Vendor")]
        public Guid? VendorId { get; set; }
    }
}
