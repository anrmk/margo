using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "CompanySectionFields")]
    public class CompanySectionFieldEntity: EntityBase<long> {
        [ForeignKey("Section")]
        [Column("Section_Id")]
        public long? CompanySectionId { get; set; }
        public virtual CompanySectionEntity Section { get; set; }

        public SectionFieldEnum Type { get; set; }

        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Value { get; set; }

        [MaxLength(128)]
        [PasswordPropertyText]
        public string Secret { get; set; }

        [MaxLength(2048)]
        public string Note { get; set; }

        [Url]
        public string Link { get; set; }
    }
}