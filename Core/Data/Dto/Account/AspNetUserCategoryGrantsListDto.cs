using System.Collections.Generic;

namespace Core.Data.Dto {
    public class AspNetUserCategoryGrantsListDto {
        public string UserId { get; set; }
        public ICollection<AspNetUserCategoryGrantsDto> Grants { get; set; }
    }
}
