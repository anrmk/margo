﻿using System.ComponentModel.DataAnnotations;

using Core.Data.Enums;
using Core.Extension;

namespace Web.ViewModels {
    public class SectionFieldViewModel {
        public long Id { get; set; }

        [Required]
        [MaxLength(24)]
        public string Name { get; set; }

        public FieldEnum Type { get; set; }
        public string TypeName => Type.GetAttribute<DisplayAttribute>().Name;

        public bool IsRequired { get; set; }

        [Required]
        public long SectionId { get; set; }
    }
}
