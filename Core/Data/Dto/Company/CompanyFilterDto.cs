using System;

namespace Core.Data.Dto {
    public class CompanyFilterDto: PagerFilterDto {
        public Guid? CEOId { get; set; }

        public string UserId { get; set; }
    }
}
