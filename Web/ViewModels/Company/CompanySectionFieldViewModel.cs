using System;
using System.ComponentModel.DataAnnotations;

using Core.Data.Enums;

namespace Web.ViewModels {
    public class CompanySectionFieldViewModel {
        [Display(Name = "Section")]
        public Guid Id { get; set; }

        [Display(Name = "Company Section")]
        public Guid CompanySectionId { get; set; }

        [Required]
        [Display(Name = "Input Type")]
        public FieldEnum Type { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Value")]
        [MaxLength(256)]
        public string Value { get; set; }

        [Display(Name = "Secret Value")]
        [MaxLength(128)]
        public string Secret { get; set; }

        [Url]
        public string Link { get; set; }

        [MaxLength(2048)]
        public string Note { get; set; }
    }
}
