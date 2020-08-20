using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "AspNetUserDenyAccessCategories")]
    public class AspNetUserDenyAccessCategoryEntity: EntityBase<Guid> {
        [ForeignKey("User")]
        [Column("User_Id")]
        [Required]
        public string UserId { get; set; }
        public virtual AspNetUserEntity User { get; set; }

        [Column("Category_Id")]
        public Guid CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; }
    }
}
