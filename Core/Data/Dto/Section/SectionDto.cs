using System.Collections.Generic;

namespace Core.Data.Dto {
    public class SectionDto {
        public long Id { get; set; }
        public int Sort { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }

        public virtual List<SectionFieldDto> Fields { get; set; }
    }
}
