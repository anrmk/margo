using System;

namespace Core.Data.Dto {
    public class LogFilterDto: PagerFilterDto {
        public long? UserId { get; set; }
        public DateTime? Date { get; set; }
    }
}
