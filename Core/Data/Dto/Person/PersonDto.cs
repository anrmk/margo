using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Dto
{
    public class PersonDto
    {
#pragma warning disable CS8618
#nullable enable

        public long Id { get; set; }

        [StringLength(64, MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(64, MinimumLength = 1)]
        public string SurName { get; set; }

        [StringLength(64)]
        public string? MiddleName { get; set; }

        [MaxLength(2048)]
        public string? Description { get; set; }

        public DateTime UpdatedDate { get; set; }

#nullable disable
#pragma warning disable CS8618
    }
}
