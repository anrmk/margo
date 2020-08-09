using System;
using System.Collections.Generic;

namespace Core.Data.Dto {
    public class VendorDto {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual List<VendorFieldDto> Fields { get; set; }
    }
}
