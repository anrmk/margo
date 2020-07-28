using System;
using System.Collections.Generic;
using Core.Data.Enums;

namespace Core.Data.Dto {
    public class UccountDto {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public long? VendorId { get; set; }
        public string VendorName { get; set; }
        public long? PersonId { get; set; }
        public UccountTypes Kind { get; set; }
        public DateTime Updated { get; set; }
        public IEnumerable<UccountServiceDto> Services { get; set; }
    }
}
