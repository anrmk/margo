using System;

namespace Core.Data.Dto {
    public class VendorDto {
        public long Id { get; set; }

        public string No { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual VendorAddressDto Address { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
