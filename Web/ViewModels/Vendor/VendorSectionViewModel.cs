using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using AutoMapper.Configuration.Annotations;

namespace Web.ViewModels {
    public class VendorSectionViewModel {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Vendor")]
        public long VendorId { get; set; }

        [Required]
        [Display(Name = "Section")]
        public long SectionId { get; set; }

        [Ignore]
        public string SectionName { get; set; }

        [Ignore]
        public string SectionCode { get; set; }

        [Ignore]
        public List<VendorSectionFieldViewModel> Fields { get; set; }
    }
}
