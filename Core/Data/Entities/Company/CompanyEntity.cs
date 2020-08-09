using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "Companies")]
    public class CompanyEntity: AuditableEntity<Guid> {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Founded { get; set; }

        public string EIN { get; set; }

        public string DB { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }
    }
}
