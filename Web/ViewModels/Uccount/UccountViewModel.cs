using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Core.Data.Enums;
using Web.Utils;

namespace Web.ViewModels {
    public class UccountViewModel {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Required]
        public UccountTypes Kind { get; set; }

        [Display(Name = "Company")]
        public Guid? CompanyId { get; set; }

        [Required]
        [Display(Name = "Vendor")]
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }

        [Display(Name = "Person")]
        public Guid? PersonId { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<UccountServiceViewModel> Services { get; set; }

        public List<UccountVendorFieldViewModel> Fields { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
