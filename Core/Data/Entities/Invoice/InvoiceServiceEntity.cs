using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "InvoiceServices")]
    public class InvoiceServiceEntity: EntityBase<Guid> {
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(24)]
        public string Name { get; set; }

        public int Count { get; set; }

        [ForeignKey("Invoice")]
        [Column("Invoice_Id")]
        public Guid InvoiceId { get; set; }
        public virtual InvoiceEntity Invoice { get; set; }
    }
}
