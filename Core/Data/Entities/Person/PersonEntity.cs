using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "Persons")]
    public class PersonEntity: AuditableEntity<Guid> {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [MaxLength(64)]
        public string SurName { get; set; }

        [MaxLength(64)]
        public string MiddleName { get; set; }

        public string FullName => Name + " " + SurName;

        [MaxLength(2048)]
        public string Description { get; set; }

        public virtual ICollection<UccountEntity> Accounts { get; set; }
    }
}
