using System.Collections.Generic;
using Core.Data.Enums;

namespace Core.Data.Dto {
    public class UccountDto {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long VendorId { get; set; }
        public UccountTypes Type { get; set; }
        public IEnumerable<UccountServiceDto> Services { get; set; }
    }
}
