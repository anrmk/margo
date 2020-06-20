using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "VendorSectionFields")]
    public class VendorSectionFieldEntity: EntityBase<long> {
        [ForeignKey("Section")]
        [Column("Section_Id")]
        public long? SectionId { get; set; }
        public virtual VendorSectionEntity Section { get; set; }

        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        [MaxLength(512)]
        public string Value { get; set; }
    }
}