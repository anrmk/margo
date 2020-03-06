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
        [Display(Name = "Subtotal")]
        public decimal Amount { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Due date")]
        public DateTime DueDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public decimal PaymentAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Business name")]
        public long? SupplierId { get; set; }
        public SupplierViewModel Supplier { get; set; }

        [Display(Name = "Company")]
        public long? CompanyId { get; set; }
        public CompanyViewModel Company { get; set; }
    }
}
