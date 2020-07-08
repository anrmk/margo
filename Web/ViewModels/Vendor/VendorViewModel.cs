using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class VendorViewModel {
        public long Id { get; set; }

        [Required]
        [MaxLength(8)]
        [Display(Name = "Supplier No")]
        public string No { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Business Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
