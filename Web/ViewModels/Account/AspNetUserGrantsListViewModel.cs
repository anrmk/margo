using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class AspNetUserGrantsListViewModel {
        [Display(Name = "User")]
        public string UserId { get; set; }
        
        public IEnumerable<AspNetUserGrantsViewModel> Grants { get; set; }
    }
}
