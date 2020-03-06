using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class CompanyListViewModel {
        public long Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class CompanyViewModel {
        public long Id { get; set; }

        #region GENERAL
        [Required]
        [MaxLength(8)]
        public string No { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        #endregion

        #region ADDRESS
        public long AddressId { get; set; }

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
        #endregion
    }
}
