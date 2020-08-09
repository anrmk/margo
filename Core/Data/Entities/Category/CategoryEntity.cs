using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "Categories")]
    public class CategoryEntity: EntityBase<Guid> {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Column("Parent_Id")]
        public Guid? ParentId { get; set; }
        public virtual CategoryEntity Parent { get; set; }

        public virtual ICollection<CategoryFieldEntity> Fields { get; set; }
    }
}
