using System;
using System.Collections.Generic;

namespace Web.ViewModels {
    public class CompanyDataMenuListViewModel {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CompanyDataMenuItemViewModel> Fields { get; set; }
    }
}
