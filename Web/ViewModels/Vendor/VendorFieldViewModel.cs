using System;
using System.ComponentModel.DataAnnotations;

using Core.Data.Enums;
using Core.Extension;

using Web.Utils;

namespace Web.ViewModels {
    public class VendorFieldViewModel {
        public Guid Id { get; set; }

        [Range(0, int.MaxValue)]
        public int Sort { get; set; }

        [Required]
        [MaxLength(24)]
        public string Name { get; set; }

        public FieldEnum Type { get; set; }
        public string TypeName => Type.GetAttribute<DisplayAttribute>().Name;
        public string HTMLTypeName => Type.GetHTMLType();

        public bool IsRequired { get; set; }
        public bool IsHidden { get; set; }

        public Guid VendorId { get; set; }
    }
}
