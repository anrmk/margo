using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "Uccounts")]
    public class UccountEntity: EntityBase<long> {
        [ForeignKey("Company")]
        [Column("Company_Id")]
        public long? CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }

        [ForeignKey("Vendor")]
        [Column("Vendor_Id")]
        public long? VendorId { get; set; }
        public virtual VendorEntity Vendor { get; set; }

        [ForeignKey("Section")]
        [Column("Section_Id")]
        public long SectionId { get; set; }
        public virtual UccountSectionEntity Section { get; set; }

    }
}
