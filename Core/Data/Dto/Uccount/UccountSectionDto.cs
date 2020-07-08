using System.Collections.Generic;

namespace Core.Data.Dto {
    public class UccountSectionDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UccountSectionFieldDto> Fields { get; set; }
    }
}
