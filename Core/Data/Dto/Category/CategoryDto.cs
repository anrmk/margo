using System.Collections.Generic;

namespace Core.Data.Dto {
    public class CategoryDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string ParentName { get; set; }

        public virtual List<CategoryFieldDto> Fields { get; set; }
    }
}
