using System;
using System.Collections.Generic;

namespace Web.ViewModels {
    public class CompanyDataListViewModel {
        public Guid CompanyId { get; set; }
        public List<CompanyDataViewModel> Data { get; set; }
    }
}
