using System;
using System.Collections.Generic;
using System.Text;

using Core.Extension;

namespace Core.Data.Dto {
    public class LogFilterDto: PagerFilter {
        public long? UserId { get; set; }
        public DateTime? Date { get; set; }
    }
}
