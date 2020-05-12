
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class CompanySectionViewModel {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Company")]
        public long CompanyId { get; set; }

        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Section")]
        public long SectionId { get; set; }

        public string SectionName { get; set; }

        public List<CompanySectionFieldViewModel> Fields { get; set; }
    }
}
