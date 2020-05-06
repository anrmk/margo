using System;

namespace Web.ViewModels {
    public class CompanyViewModel {
        public long Id { get; set; }

        public CompanyGeneralViewModel General { get; set; }

        public CompanyAddressViewModel Address { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
