using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class CompanyAddressViewModel {
        public long Id { get; set; }

        [MaxLength(60)]
        public string Address { get; set; }

        [MaxLength(60)]
        public string Address2 { get; set; }

        [MaxLength(60)]
        public string City { get; set; }

        [MaxLength(60)]
        public string State { get; set; }

        [MaxLength(10)]
        public string ZipCode { get; set; }

        [MaxLength(60)]
        public string Country { get; set; }
    }
}
