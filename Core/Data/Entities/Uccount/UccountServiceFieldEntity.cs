using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Enums;

namespace Core.Data.Entities {
    [Table(name: "UccountServiceFields")]
    public class UccountServiceFieldEntity: EntityBase<long> {
        [ForeignKey("Service")]
        [Column("Service_Id")]
        public long ServiceId { get; set; }
        public virtual UccountServiceEntity Service { get; set; }

        public FieldEnum Type { get; set; }
        [Required]
        [MaxLength(24)]
        public string Name { get; set; }
        [Required]
        [MaxLength(256)]
        public string Value { get; set; }
        public bool IsRequired { get; set; }
    }
}
