using System;

namespace Web.ViewModels {
    public class UccountListViewModel {
        public long Id { get; set; }
        public string Kind { get; set; }
        public string Name { get; set; }
        public string VendorName { get; set; }
        public int ServiceCount { get; set; }

        public DateTime Updated { get; set; }
    }
}
