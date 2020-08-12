using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "CompanySections")]
    public class CompanySectionEntity: EntityBase<Guid> {
        [ForeignKey("Company")]
        [Column("Company_Id")]
        public Guid CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }

        public string Name { get; set; }

        public virtual ICollection<CompanySectionFieldEntity> Fields { get; set; }
    }
}
