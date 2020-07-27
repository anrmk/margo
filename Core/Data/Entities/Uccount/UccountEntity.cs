using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Data.Enums;

namespace Core.Data.Entities {
    [Table(name: "Uccounts")]
    public class UccountEntity: AuditableEntity<long> {
        [ForeignKey("Company")]
        [Column("Company_Id")]
        public long? CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }

        [ForeignKey("Vendor")]
        [Column("Vendor_Id")]
        public long? VendorId { get; set; }
        public virtual VendorEntity Vendor { get; set; }

        [ForeignKey("Person")]
        [Column("Person_Id")]
        public long? PersonId { get; set; }
        public virtual UccountPersonEntity Person { get; set; }

        public virtual ICollection<UccountServiceEntity> Services { get; set; }

        public UccountTypes Kind { get; set; }

        public virtual ICollection<UccountSectionEntity> Sections { get; set; }
    }
}
