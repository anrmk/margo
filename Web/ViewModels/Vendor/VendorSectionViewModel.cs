using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels {
    public class VendorSectionViewModel {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Vendor")]
        public long VendorId { get; set; }

        [Required]
        [Display(Name = "Section")]
        public long SectionId { get; set; }

        public string SectionName { get; set; }
        public string SectionCode { get; set; }

        public List<VendorSectionFieldViewModel> Fields { get; set; }
    }
}
