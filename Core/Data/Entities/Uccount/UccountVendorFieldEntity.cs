using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Enums;

namespace Core.Data.Entities {
    [Table(name: "UccountVendorFields")]
    public class UccountVendorFieldEntity: EntityBase<Guid> {
        [ForeignKey("Vendor")]
        [Column("Vendor_Id")]
        public Guid VendorId { get; set; }

        [ForeignKey("Account")]
        [Column("Account_Id")]
        public Guid AccountId { get; set; }
        public virtual UccountEntity Account { get; set; }

        public FieldEnum Type { get; set; }

        [Required]
        [MaxLength(24)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Value { get; set; }

        public bool IsRequired { get; set; }
    }
}
