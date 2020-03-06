using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.ViewModels {
    public class SupplierViewModel {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        [MaxLength(16)]
        public string No { get; set; }

        [Required]
        [Display(Name = "Business Name")]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Company")]
        public long? CompanyId { get; set; }

        #region Address
        public long? AddressId { get; set; }

        [MaxLength(60)]
        public string Address { get; set; }

        [Display(Name = "Address 2")]
        [MaxLength(60)]
        public string Address2 { get; set; }

        [MaxLength(60)]
        public string City { get; set; }

        [MaxLength(60)]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        [MaxLength(10)]
        public string ZipCode { get; set; }

        [MaxLength(60)]
        public string Country { get; set; }
        #endregion
    }
}
