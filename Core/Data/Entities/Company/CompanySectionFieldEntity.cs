using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Enums;

namespace Core.Data.Entities {
    [Table(name: "CompanySectionFields")]
    public class CompanySectionFieldEntity: EntityBase<Guid> {
        [ForeignKey("Section")]
        [Column("Section_Id")]
        public Guid SectionId { get; set; }
        public virtual CompanySectionEntity Section { get; set; }

        public FieldEnum Type { get; set; }
        [Required]
        [MaxLength(24)]
        public string Name { get; set; }
        [Required]
        [MaxLength(256)]
        public string Value { get; set; }
        public bool IsRequired { get; set; }
        public int Sort { get; set; }
        public bool IsHidden { get; set; }
    }
}
