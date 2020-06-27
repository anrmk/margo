using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "UccountSectionFields")]
    public class UccountSectionFieldEntity: EntityBase<long> {
        [Required]
        [MaxLength(24)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Value { get; set; }
    }
}
