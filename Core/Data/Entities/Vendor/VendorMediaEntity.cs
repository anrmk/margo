using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities.Vendor {
    [Table(name: "VendorMedias")]
    public class VendorMediaEntity: MediaEntity {
        [ForeignKey("Vendor")]
        [Column("Vendor_Id")]
        public Guid? VendorId { get; set; }
        public virtual VendorEntity Vendor { get; set; }

        /// <summary>
        /// Миниатюра изображения в массиве байтов
        /// </summary>
        public byte[] Thumbnail { get; set; }
    }
}
