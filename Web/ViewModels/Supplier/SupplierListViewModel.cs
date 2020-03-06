using System;

namespace Web.ViewModels {
    public class SupplierListViewModel {
        public long Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Terms { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public double CreditLimit { get; set; }
        public double CreditUtilized { get; set; }

        public long CompanyId { get; set; }
        public string Company { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
