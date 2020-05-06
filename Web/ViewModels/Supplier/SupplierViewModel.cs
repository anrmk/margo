using System;

namespace Web.ViewModels {
    public class SupplierViewModel {
        public long Id { get; set; }

        public SupplierGeneralViewModel General { get; set; }

        public SupplierAddressViewModel Address { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
