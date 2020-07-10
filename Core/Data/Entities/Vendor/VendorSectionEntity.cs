using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "VendorSectionEntity")]
    public class VendorSectionEntity: EntityBase<long> {
        [ForeignKey("Vendor")]
        [Column("Vendor_Id")]
        public long? VendorId { get; set; }
        public virtual VendorEntity Vendor { get; set; }

        [Column("Section_Id")]
        public long? SectionId { get; set; }
        public virtual SectionEntity Section { get; set; }

        public virtual ICollection<VendorFieldEntity> Fields { get; set; }
    }
}