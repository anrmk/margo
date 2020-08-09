using System;
using System.Collections.Generic;

namespace Core.Data.Dto {
    public class CategoryDto {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string ParentName { get; set; }

        public virtual List<CategoryFieldDto> Fields { get; set; }
    }
}
