using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class InvoiceViewModel {
        public long Id { get; set; }

        [Required]
        [MaxLength(16)]
        [Display(Name = "Invoice No")]
        public string No { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Tax Rate")]
        public decimal TaxRate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due date")]
        public DateTime DueDate { get; set; }

        public bool IsPayd { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Payment date")]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Vendor")]
        public long? VendorId { get; set; }

        [Display(Name = "Company")]
        public long? CompanyId { get; set; }

        public bool IsDraft { get; set; }
    }
}
