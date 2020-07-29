using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities
{
    [Table(name: "Services")]
    public class UccountServiceEntity : EntityBase<long>
    {
        [ForeignKey("Account")]
        [Column("Account_Id")]
        public long AccountId { get; set; }
        public virtual UccountEntity Account { get; set; }
        [ForeignKey("Category")]
        [Column("Category_Id")]
        public long CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; }
        public virtual ICollection<UccountServiceFieldEntity> Fields { get; set; }

        public UccountServiceEntity() {
            Fields = new HashSet<UccountServiceFieldEntity>();
        }
    }
}
