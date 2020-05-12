using System;

namespace Web.ViewModels {
    public class CompanyListViewModel {
        public long Id { get; set; }

        public string IsStarred { get; set; }

        public string No { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
