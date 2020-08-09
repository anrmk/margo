﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "Vendors")]
    public class VendorEntity: AuditableEntity<Guid> {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        public virtual ICollection<VendorFieldEntity> Fields { get; set; }
    }
}
