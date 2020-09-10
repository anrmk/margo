using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "Invoices")]
    public class InvoiceEntity: AuditableEntity<Guid> {
        [Required]
        [MaxLength(16)]
        public string No { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Range(0, 100)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TaxRate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public bool IsDraft { get; set; } = true;

        [Required]
        [ForeignKey("Account")]
        [Column("Account_Id")]
        public Guid AccountId { get; set; }
        public virtual UccountEntity Account { get; set; }

        public virtual ICollection<PaymentEntity> Payments { get; set; }

        public virtual ICollection<InvoiceServiceEntity> Services { get; set; }
    }
}