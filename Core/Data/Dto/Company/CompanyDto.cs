using System;

namespace Core.Data.Dto {
    public class CompanyDto {
        public long Id { get; set; }

        public string No { get; set; }
        public string Name { get; set; }
       
        public DateTime? Founded { get; set; }
        public string EIN { get; set; }
        public string DB { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
