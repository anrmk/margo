using System;
using System.Collections.Generic;

namespace Core.Data.Dto {
    public class CompanyDataListDto {
        public Guid CompanyId { get; set; }
        public List<CompanyDataDto> Data { get; set; }
    }
}
