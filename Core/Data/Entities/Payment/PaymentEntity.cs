using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "Payments")]
    public class PaymentEntity: AuditableEntity<long> {
        [Required]
        public string No { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public PaymentMethodEnum Method { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [MaxLength(2048)]
        public string Note { get; set; }

        [Column("Invoice_Id")]
        public long? InvoiceId { get; set; }
        public virtual InvoiceEntity Invoice { get; set; }
    }
}
