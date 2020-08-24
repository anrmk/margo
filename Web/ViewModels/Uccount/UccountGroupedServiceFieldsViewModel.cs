using System;
using System.Collections.Generic;

namespace Web.ViewModels {
    public class UccountGroupedServiceFieldsViewModel {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }

        public IEnumerable<UccountServiceFieldViewModel> Fields { get; set; }
    }
}
