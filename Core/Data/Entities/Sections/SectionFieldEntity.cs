using System.ComponentModel.DataAnnotations;

namespace Core.Data.Entities {
    public class SectionFieldEntity: AuditableEntity<long> {
        public SectionFieldEnum Type { get; set; }

        [Required]
        [MaxLength(24)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Value { get; set; }
    }
}
