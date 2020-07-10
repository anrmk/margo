using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Core.Data.Enums;
using Core.Extension;

namespace Web.ViewModels {
    public class CategoryFieldViewModel {
        public long Id { get; set; }

        [Required]
        [MaxLength(24)]
        public string Name { get; set; }

        public FieldEnum Type { get; set; }
        public string TypeName => Type.GetAttribute<DisplayAttribute>().Name;

        public bool IsRequired { get; set; }

        public long CategoryId { get; set; }
    }
}
