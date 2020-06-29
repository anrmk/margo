using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Entities {
    public class UccountSectionEntity: EntityBase<long> {
        public int Sort { get; set; }

        [Required]
        [MaxLength(24)]
        public string Name { get; set; }

        public string Code { get; set; }

        [MaxLength(2048)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public bool IsDefault { get; set; }

        public virtual ICollection<UccountSectionFieldEntity> Fields { get; set; }
    }
}
