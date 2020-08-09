using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Enums;

namespace Core.Data.Entities {
    [Table(name: "CategoryFields")]
    public class CategoryFieldEntity: EntityBase<Guid> {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        public FieldEnum Type { get; set; }

        public bool IsRequired { get; set; }

        public int Sort { get; set; }

        public bool IsHidden { get; set; }

        [ForeignKey("Category")]
        [Column("Category_Id")]
        public Guid CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; }
    }
}
