using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Enums;

namespace Core.Data.Entities {
    public class SectionFieldEntity: EntityBase<long> {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        public FieldEnum Type { get; set; }

        public bool IsRequired { get; set; }

        [ForeignKey("Section")]
        [Column("Section_Id")]
        public long SectionId { get; set; }
        public virtual SectionEntity Section { get; set; }
    }
}
