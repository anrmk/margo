using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels {
    public class CompanyFilterViewModel: PagerFilterViewModel {
        [Display(Name = "CEO")]
        public Guid? CEOId { get; set; }
    }
}
