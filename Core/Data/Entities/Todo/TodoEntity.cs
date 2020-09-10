using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Data.Enums;

namespace Core.Data.Entities {
    [Table(name: "Todo")]
    public class TodoEntity: AuditableEntity<Guid> {
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Description { get; set; }

        [Range(1, 3)]
        public TodoPriorityEnum Priority { get; set; }

        public bool IsCompleted { get; set; }

        [Required]
        [Column("User_Id")]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AspNetUserEntity User { get; set; }
    }
}
