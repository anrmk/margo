using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "Companies")]
    public class CompanyEntity: AuditableEntity<long> {
        [Required]
        [MaxLength(8)]
        public string No { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        //[DataType(DataType.PhoneNumber)]
        //public string PhoneNumber { get; set; }

        //[DataType(DataType.Url)]
        //public string Website { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Founded { get; set; }

        public string EIN { get; set; }

        public string DB { get; set; }

        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }

        //public string CEO { get; set; }

        //[ForeignKey("Address")]
        //[Column("CompanyAddress_Id")]
        //public long? AddressId { get; set; }
        //public virtual CompanyAddressEntity Address { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }
    }
}
