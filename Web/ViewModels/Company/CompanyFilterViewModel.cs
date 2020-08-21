using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class CompanyFilterViewModel: PagerFilterViewModel {
        [Display(Name = "CEO")]
        public Guid? CEOId { get; set; }
    }
}
