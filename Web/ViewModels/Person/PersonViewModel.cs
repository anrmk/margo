﻿using System;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618
#nullable enable
namespace Web.ViewModels
{
    public class PersonViewModel
    {
        public long Id { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string SurName { get; set; }

        [StringLength(64)]
        public string? MiddleName { get; set; }

        [MaxLength(2048)]
        public string? Description { get; set; }
    }
}
#nullable disable
#pragma warning disable CS8618
