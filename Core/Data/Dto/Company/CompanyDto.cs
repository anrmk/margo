using System;
using System.Collections.Generic;

namespace Core.Data.Dto {
    public class CompanyDto {
        public Guid Id { get; set; }

        public Guid CEOId { get; set; }
        public string CEOName { get; set; }

        public string Name { get; set; }

        public DateTime? Founded { get; set; }
        public string EIN { get; set; }
        public string DB { get; set; }

        public string Description { get; set; }

        public virtual ICollection<CompanySectionDto> Sections { get; set; }
        public virtual ICollection<CompanyDataDto> Data { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
