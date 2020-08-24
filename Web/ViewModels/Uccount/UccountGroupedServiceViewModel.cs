using System;
using System.Collections.Generic;

namespace Web.ViewModels {
    public class UccountGroupedServiceViewModel {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? CategoryId { get; set; }

        public IEnumerable<UccountGroupedServiceFieldsViewModel> Groups { get; set; }
    }
}
