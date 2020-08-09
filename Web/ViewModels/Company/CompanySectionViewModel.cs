using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AutoMapper.Configuration.Annotations;

namespace Web.ViewModels {
    public class CompanySectionViewModel {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Company")]
        public Guid CompanyId { get; set; }

        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Section")]
        public long SectionId { get; set; }

        [Ignore]
        public string SectionName { get; set; }

        [Ignore]
        public string SectionCode { get; set; }

        [Ignore]
        public string SectionDescription { get; set; }

        [Ignore]
        public List<CompanySectionFieldViewModel> Fields { get; set; }
    }
}
