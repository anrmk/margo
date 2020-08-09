using System.Collections.Generic;

namespace Core.Data.Dto {
    public class PagerFilterDto {
        public string Search { get; set; }
        // public int Skip { get; set; }
        public int Length { get; set; }
        public int Start { get; set; }
        public List<PagerSortQuery> Sort { get; set; }
    }

    public class PagerSortQuery {
        public bool Desc { get; set; } = false;
        public string Selector { get; set; }
    }
}
