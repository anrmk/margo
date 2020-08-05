using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "UccountServices")]
    public class UccountServiceEntity: EntityBase<long> {
        [ForeignKey("Account")]
        [Column("Account_Id")]
        public long AccountId { get; set; }
        public virtual UccountEntity Account { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UccountServiceFieldEntity> Fields { get; set; }
    }
}
