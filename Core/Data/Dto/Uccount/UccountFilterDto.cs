using System;

using Core.Data.Enums;

namespace Core.Data.Dto {
    public class UccountFilterDto: PagerFilterDto {
        public UccountTypes? Kind { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? VendorId { get; set; }
        public Guid? CategoryId { get; set; }

        public string UserId { get; set; }
    }
}
