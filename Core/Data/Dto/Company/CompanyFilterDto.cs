using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Data.Dto {
    public class CompanyFilterDto: PagerFilterDto {
        public Guid? CEOId { get; set; }
    }
}
