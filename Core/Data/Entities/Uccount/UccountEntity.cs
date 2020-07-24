using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Data.Enums;

namespace Core.Data.Entities {
    [Table(name: "Uccounts")]
    public class UccountEntity: EntityBase<long> {
        [ForeignKey("Company")]
        [Column("Company_Id")]
        public long CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }

        [ForeignKey("Vendor")]
        [Column("Vendor_Id")]
        public long VendorId { get; set; }
        public virtual VendorEntity Vendor { get; set; }

        public virtual ICollection<ServiceEntity> Services { get; set; }

        public UccountTypes Type { get; set; }

        public virtual ICollection<UccountSectionEntity> Sections { get; set; }
    }
}
