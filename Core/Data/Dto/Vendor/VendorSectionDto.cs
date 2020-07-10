using System.Collections.Generic;

namespace Core.Data.Dto {
    public class VendorSectionDto {
        public long Id { get; set; }

        public long VendorId { get; set; }
        public string VendorName { get; set; }

        public long SectionId { get; set; }
        public string SectionName { get; set; }
        public string SectionCode { get; set; }

        public virtual List<VendorFieldDto> Fields { get; set; }
    }
}
