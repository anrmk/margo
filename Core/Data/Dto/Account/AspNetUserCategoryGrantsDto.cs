using System;

namespace Core.Data.Dto {
    public class AspNetUserCategoryGrantsDto {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid? CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public bool IsGranted { get; set; }
    }
}
