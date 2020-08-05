﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class CompanyViewModel {
        public long Id { get; set; }

        [Required]
        [MaxLength(8)]
        [Display(Name = "Company No")]
        public string No { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Business Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Founded")]
        [DataType(DataType.Date)]
        public DateTime Founded { get; set; }
        
        [Required]
        [MaxLength(60)]
        [Display(Name = "EIN")]
        public string EIN { get; set; }

        [Required]
        [MaxLength(60)]
        [Display(Name = "D&B")]
        public string DB { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
