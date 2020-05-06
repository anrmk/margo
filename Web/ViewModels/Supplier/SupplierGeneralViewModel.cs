using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class SupplierGeneralViewModel {
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

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [MaxLength(60)]
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(60)]
        [Display(Name = "Web site")]
        [DataType(DataType.Url)]
        public string Website { get; set; }
    }
}
