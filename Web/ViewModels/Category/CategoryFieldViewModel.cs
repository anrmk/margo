using System;
using System.ComponentModel.DataAnnotations;

using Core.Data.Enums;
using Core.Extension;
using Web.Utils;

namespace Web.ViewModels {
    public class CategoryFieldViewModel {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(24)]
        public string Name { get; set; }

        public FieldEnum Type { get; set; }
        public string TypeName => Type.GetAttribute<DisplayAttribute>().Name;
        public string HTMLTypeName => Type.GetHTMLType();

        public bool IsRequired { get; set; }

        public Guid CategoryId { get; set; }
    }
}
