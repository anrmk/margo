using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.ViewModels {
    public class InvoiceServiceViewModel {
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public int Count { get; set; }

        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        //public Guid CategoryId { get; set; }
        public Guid InvoiceId { get; set; }
    }
}
