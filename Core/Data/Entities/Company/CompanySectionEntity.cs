using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "CompanySections")]
    public class CompanySectionEntity: EntityBase<long> {
        [ForeignKey("Company")]
        [Column("Company_Id")]
        public long? CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }

        [Column("Section_Id")]
        public long? SectionId { get; set; }
        public virtual SectionEntity Section { get; set; }

        public virtual ICollection<CompanySectionFieldEntity> Fields { get; set; }
    }
}