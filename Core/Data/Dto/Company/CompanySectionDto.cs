using System.Collections.Generic;

namespace Core.Data.Dto {
    public class CompanySectionDto {
        public long Id { get; set; }

        public long CompanyId { get; set; }
        public string CompanyName { get; set; }

        public long SectionId { get; set; }
        public string SectionName { get; set; }
        public string SectionCode { get; set; }
        public string SectionDescription { get; set; }

        public virtual List<CompanySectionFieldDto> Fields { get; set; }
    }
}
