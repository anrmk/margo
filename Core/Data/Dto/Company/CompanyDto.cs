using System;

namespace Core.Data.Dto {
    public class CompanyDto {
        public long Id { get; set; }

        public string IsStarred { get; set; }

        public CompanyGeneralDto General { get; set; }

        public virtual CompanyAddressDto Address { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
