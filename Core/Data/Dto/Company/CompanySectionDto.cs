using System;
using System.Collections.Generic;

namespace Core.Data.Dto {
    public class CompanySectionDto {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }

        public Guid SectionId { get; set; }
        public string SectionName { get; set; }
        public string SectionCode { get; set; }
        public string SectionDescription { get; set; }

        public virtual List<CompanySectionFieldDto> Fields { get; set; }
    }
}
