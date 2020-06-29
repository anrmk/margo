using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "UccountSectionFields")]
    public class UccountSectionFieldEntity: EntityBase<long> {
        [ForeignKey("Section")]
        [Column("Section_Id")]
        public long SectionId { get; set; }
        public UccountSectionEntity Section { get; set; }

        [Required]
        [MaxLength(24)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Value { get; set; }
    }
}
