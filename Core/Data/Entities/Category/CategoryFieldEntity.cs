using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Enums;

namespace Core.Data.Entities {
    [Table(name: "CategoryFields")]
    public class CategoryFieldEntity: EntityBase<long> {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        public FieldEnum Type { get; set; }

        public bool IsRequired { get; set; }

        [ForeignKey("Category")]
        [Column("Category_Id")]
        public long CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; }
    }
}
