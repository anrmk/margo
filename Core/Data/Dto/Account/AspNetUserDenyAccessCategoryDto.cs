using System;

namespace Core.Data.Dto {
    public class AspNetUserDenyAccessCategoryDto {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public Guid? CategoryId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
