using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class InvoiceViewModel {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(16)]
        [Display(Name = "Invoice No")]
        public string No { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Tax Rate")]
        [Range(0, 100)]
        public decimal TaxRate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due date")]
        public DateTime DueDate { get; set; }

        public bool IsDraft { get; set; } = false;

        public IEnumerable<InvoiceServiceViewModel> Services { get; set; }

        [Display(Name = "Account")]
        public Guid AccountId { get; set; }
    }
}
