using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "Uccounts")]
    public class UccountEntity: EntityBase<long> {
        public long CompanyId { get; set; }
        public long VendorId { get; set; }
        public long ServiceId { get; set; }
    }
}
