using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class UccountViewModel {
        public long Id { get; set; }

        [Display(Name ="Company")]
        public long CompanyId { get; set; }
        [Display(Name = "Vendor")]
        public long VendorId { get; set; }
        public int Kind { get; set; }
        public IEnumerable<UccountServiceViewModel> Services { get; set; }
    }
}
