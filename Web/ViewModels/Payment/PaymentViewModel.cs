using System;
using System.ComponentModel.DataAnnotations;

using Core.Data.Enums;
using Core.Extension;

namespace Web.ViewModels {
    public class PaymentViewModel {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Reference no.")]
        public string No { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Payment date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Amount received")]
        public decimal Amount { get; set; }

        [Display(Name = "Payment method")]
        public PaymentMethodEnum Method { get; set; }

        [Display(Name = "Payment method")]
        public string MethodName =>
            Method.GetAttribute<DisplayAttribute>().Name;

        [Display(Name = "Memo")]
        public string Note { get; set; }

        [Display(Name = "Invoice")]
        public Guid InvoiceId { get; set; }

        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }
    }
}
