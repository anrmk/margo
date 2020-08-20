using System;
using System.Collections.Generic;

using Core.Data.Enums;

namespace Core.Data.Dto {
    public class UccountDto {
        public Guid Id { get; set; }

        public Guid? CompanyId { get; set; }

        public string Name { get; set; }

        public Guid VendorId { get; set; }
        public string VendorName { get; set; }

        public Guid? PersonId { get; set; }

        public UccountTypes Kind { get; set; }

        public ICollection<UccountServiceDto> Services { get; set; }
        public ICollection<UccountVendorFieldDto> Fields { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
