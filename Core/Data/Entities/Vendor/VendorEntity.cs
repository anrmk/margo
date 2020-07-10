using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "Vendors")]
    public class VendorEntity: AuditableEntity<long> {
        [Required]
        [MaxLength(16)]
        public string No { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        public virtual ICollection<VendorFieldEntity> Fields { get; set; }

        //[DataType(DataType.PhoneNumber)]
        //public string PhoneNumber { get; set; }

        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }

        //[DataType(DataType.Url)]
        //public string Website { get; set; }

        //[ForeignKey("Address")]
        //[Column("VendorAddress_Id")]
        //public long? AddressId { get; set; }
        //public virtual VendorAddressEntity Address { get; set; }
    }
}
