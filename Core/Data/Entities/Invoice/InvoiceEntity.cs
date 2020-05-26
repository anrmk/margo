using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.Data.Entities {
    [Table(name: "Invoices")]
    public class InvoiceEntity: AuditableEntity<long> {
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

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [NotMapped]
        public bool IsPayd {
            get {
                if(Payments != null && Payments.Count > 0) {
                    var result = Payments.Sum(x => x.Amount);
                    return Amount == result;
                }
                return false;
            }
        }

        [NotMapped]
        public DateTime? PaymentDate {
            get {
                if(Payments != null && Payments.Count > 0) {
                    var result = Payments.OrderByDescending(x => x.Date).FirstOrDefault();
                    return result?.Date;
                }
                return (DateTime?)null;
            }
        }

        [NotMapped]
        public decimal? PaymentAmount {
            get {
                if(Payments != null && Payments.Count > 0) {
                    var result = Payments.Sum(x => x.Amount);
                    return result;
                }
                return (decimal?)null;
            }
        }

        public bool IsDraft { get; set; } = true;

        [ForeignKey("Company")]
        [Column("Company_Id")]
        public long? CompanyId { get; set; }
        public CompanyEntity Company { get; set; }

        [ForeignKey("Vendor")]
        [Column("Vendor_Id")]
        public long? VendorId { get; set; }
        public VendorEntity Vendor { get; set; }

        public virtual List<PaymentEntity> Payments { get; set; }
    }
}