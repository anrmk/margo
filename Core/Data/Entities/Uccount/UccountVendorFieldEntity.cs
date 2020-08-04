using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Enums;

namespace Core.Data.Entities {
    [Table(name: "UccountVendorFields")]
    public class UccountVendorFieldEntity: EntityBase<long> {
        [ForeignKey("Vendor")]
        [Column("Vendor_Id")]
        public long VendorId { get; set; }
        [ForeignKey("Account")]
        [Column("Account_Id")]
        public long AccountId { get; set; }
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
