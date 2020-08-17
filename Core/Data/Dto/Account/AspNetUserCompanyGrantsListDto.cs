using System.Collections.Generic;

namespace Core.Data.Dto {
    public class AspNetUserCompanyGrantsListDto {
        public string UserId { get; set; }
        public ICollection<AspNetUserCompanyGrantsDto> Grants { get; set; }
    }
}
