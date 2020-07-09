using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels {
    public class CategoryViewModel {
        public long Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public long? ParentId { get; set; }

    }
}
