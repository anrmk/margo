using System;
using System.ComponentModel.DataAnnotations;
using Core.Data.Enums;

namespace Web.ViewModels {
    public class InvoiceFilterViewModel: PagerFilterViewModel {
        [Display(Name = "Type")]
        public UccountTypes? Kind { get; set; }

        [Display(Name = "Customer")]
        public Guid? CustomerId { get; set; }

        [Display(Name = "Vendor")]
        public Guid? VendorId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Display(Name = "Unpaid")]
        public bool Unpaid { get; set; }
    }
}
