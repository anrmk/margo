using System;
using System.Collections.Generic;

namespace Core.Data.Dto {
    public class CompanySectionDto {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public IEnumerable<CompanySectionFieldDto> Fields { get; set; }
    }
}
