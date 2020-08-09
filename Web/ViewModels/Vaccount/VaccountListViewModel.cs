using System;

namespace Web.ViewModels {
    public class VaccountListViewModel {
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string VendorName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
