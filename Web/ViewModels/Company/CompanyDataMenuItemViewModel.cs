using System;

namespace Web.ViewModels {
    public class CompanyDataMenuItemViewModel {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
        public Guid[] Ids { get; set; }
    }
}
