using System.Collections.Generic;

namespace Core.Data.Dto {
    public class UccountServiceDto {
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<UccountSectionDto> Sections { get; set; }
    }
}
