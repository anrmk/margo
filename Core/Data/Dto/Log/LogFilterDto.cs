using System;

using Core.Extension;

namespace Core.Data.Dto {
    public class LogFilterDto: PagerFilter {
        public long? UserId { get; set; }
        public DateTime? Date { get; set; }
    }
}
