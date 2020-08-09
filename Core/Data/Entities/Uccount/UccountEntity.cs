using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Enums;

namespace Core.Data.Entities {
    [Table(name: "Uccounts")]
    public class UccountEntity: AuditableEntity<Guid> {
        [ForeignKey("Company")]
        [Column("Company_Id")]
        public Guid? CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }

        [ForeignKey("Vendor")]
        [Column("Vendor_Id")]
        public Guid VendorId { get; set; }
        public virtual VendorEntity Vendor { get; set; }

        [ForeignKey("Person")]
        [Column("Person_Id")]
        public Guid? PersonId { get; set; }
        public virtual PersonEntity Person { get; set; }

        public UccountTypes Kind { get; set; }

        public virtual ICollection<UccountServiceEntity> Services { get; set; }

        public virtual ICollection<InvoiceEntity> Invoices { get; set; }

        public virtual ICollection<UccountVendorFieldEntity> Fields { get; set; }
    }
}