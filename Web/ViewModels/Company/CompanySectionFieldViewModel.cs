using System.ComponentModel.DataAnnotations;

using Core.Data.Entities;

namespace Web.ViewModels {
    public class CompanySectionFieldViewModel {
        [Display(Name = "Section")]
        public long Id { get; set; }

        [Display(Name = "Company Section")]
        public long CompanySectionId { get; set; }

        [Required]
        [Display(Name = "Input Type")]
        public SectionFieldEnum Type { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Value")]
        [MaxLength(256)]
        public string Value { get; set; }

        [Url]
        public string Link { get; set; }

        [MaxLength(2048)]
        public string Note { get; set; }
    }
}
