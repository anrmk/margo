using System;
using System.ComponentModel.DataAnnotations;

using Core.Data.Enums;

namespace Web.ViewModels {
    public class UccountFilterViewModel: PagerFilterViewModel {
        [Display(Name = "Type")]
        public UccountTypes? Kind { get; set; }

        [Display(Name = "Customer")]
        public Guid? CustomerId { get; set; }

        [Display(Name = "Vendor")]
        public Guid? VendorId { get; set; }
    }
}
