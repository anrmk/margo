using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class VendorViewModel {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Business Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        [Display(Name = "Categories")]
        public virtual List<Guid> Categories { get; set; }
        [Display(Name = "Fields")]
        public virtual List<VendorFieldViewModel> Fields { get; set; }
    }
}