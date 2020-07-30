using System.Collections.Generic;

namespace Core.Data.Dto {
    public class UccountServiceDto {
        public long Id { get; set; }
        public long UccountId { get; set; }
        public IEnumerable<UccountServiceFieldDto> Fields { get; set; }
    }
}
