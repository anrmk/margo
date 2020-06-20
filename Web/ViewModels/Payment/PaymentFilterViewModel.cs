using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class PaymentFilterViewModel: PagerFilterViewModel {
        [Display(Name = "Invoice")]
        public long? InvoiceId { get; set; }

        [Display(Name = "Date From")]
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "Date To")]
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }
    }
}