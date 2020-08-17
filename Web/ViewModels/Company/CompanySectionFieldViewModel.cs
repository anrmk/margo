using System;
using System.ComponentModel.DataAnnotations;

using Core.Data.Enums;
using Core.Extension;

using Web.Utils;

namespace Web.ViewModels {
    public class CompanySectionFieldViewModel {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(24)]
        public string Name { get; set; }
        [Required]
        [MaxLength(256)]
        public string Value { get; set; } = "";
        public FieldEnum Type { get; set; }
        public string TypeName => Type.GetAttribute<DisplayAttribute>().Name.ToLower();
        public string HTMLTypeName => Type.GetHTMLType();
        public bool IsRequired { get; set; }
        public int Sort { get; set; }
        public bool IsHidden { get; set; }
    }
}
