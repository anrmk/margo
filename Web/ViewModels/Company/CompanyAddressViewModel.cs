using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class CompanyAddressViewModel {
        public long Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Address { get; set; }

        [MaxLength(60)]
        public string Address2 { get; set; }

        [Required]
        [MaxLength(60)]
        public string City { get; set; }

        [Required]
        [MaxLength(60)]
        public string State { get; set; }

        [Required]
        [MaxLength(10)]
        public string ZipCode { get; set; }

        [Required]
        [MaxLength(60)]
        public string Country { get; set; }
    }
}
