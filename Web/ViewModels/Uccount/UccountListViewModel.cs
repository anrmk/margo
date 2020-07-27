using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class UccountListViewModel {
        public long Id { get; set; }
        public string Kind { get; set; }
        public string Name { get; set; }
        public int ServiceCount { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
