using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "VendorCategories")]
    public class VendorCategoryEntity: EntityBase<Guid> {
        [ForeignKey("Category")]
        [Column("Category_Id")]
        public Guid CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; }

        [ForeignKey("Vendor")]
        [Column("Vendor_Id")]
        public Guid VendorId { get; set; }
        public virtual VendorEntity Vendor { get; set; }
    }
}