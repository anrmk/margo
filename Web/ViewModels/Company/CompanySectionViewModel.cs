using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class CompanySectionViewModel {
        public Guid Id { get; set; }
        [Display(Name = "Company")]
        public Guid? CompanyId { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public IEnumerable<CompanySectionFieldViewModel> Fields { get; set; }
    }
}
