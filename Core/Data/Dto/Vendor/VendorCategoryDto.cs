using System;
using System.Collections.Generic;

namespace Core.Data.Dto {
    public class VendorCategoryDto {
        public Guid Id { get; set; }
        public Guid VendorId { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual List<CategoryFieldDto> Fields { get; set; }
    }
}