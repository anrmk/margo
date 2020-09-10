using System;
using Core.Data.Enums;

namespace Core.Data.Dto {
    public class TodoDto {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public TodoPriorityEnum Priority { get; set; }
        public bool IsCompleted { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
