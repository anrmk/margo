using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Enums;

namespace Core.Data.Entities {
    [Table(name: "VendorFields")]
    public class VendorFieldEntity: EntityBase<Guid> {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        public FieldEnum Type { get; set; }

        public bool IsRequired { get; set; }

        public int Sort { get; set; }

        public bool IsHidden { get; set; }

        [ForeignKey("Vendor")]
        [Column("Vendor_Id")]
        public Guid VendorId { get; set; }
        public virtual VendorEntity Vendor { get; set; }
    }
}