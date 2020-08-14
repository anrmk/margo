using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class AspNetUserCompanyGrantsListViewModel {
        [Display(Name = "User")]
        public string UserId { get; set; }
        
        public IEnumerable<AspNetUserCompanyGrantsViewModel> Grants { get; set; }
    }
}
