using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "CompanyData")]
    public class CompanyDataEntity: EntityBase<Guid> {
        [ForeignKey("Company")]
        [Column("Company_Id")]
        public Guid CompanyId { get; set; }

        [ForeignKey("UccountServiceField")]
        [Column("UccountServiceField_Id")]
        public Guid FieldId { get; set; }
        public virtual UccountServiceFieldEntity Field { get; set; }
    }
}
