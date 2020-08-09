using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class CategoryViewModel {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
        public string ParentName { get; set; }

        public virtual List<CategoryFieldViewModel> Fields { get; set; }
    }
}
