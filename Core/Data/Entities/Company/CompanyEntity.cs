using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "Companies")]
    public class CompanyEntity: AuditableEntity<Guid> {
        [ForeignKey("Person")]
        [Column("Person_Id")]
        public Guid CEOId { get; set; }
        public virtual PersonEntity CEO { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Founded { get; set; }

        [Required]
        [MaxLength(60)]
        public string EIN { get; set; }

        [Required]
        [MaxLength(60)]
        public string DB { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        public virtual ICollection<CompanySectionEntity> Sections { get; set; }
        public virtual ICollection<CompanyDataEntity> Data { get; set; }

        public virtual ICollection<AspNetUserGrantEntity> Grants { get; set; }
    }
}
