using System;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618
#nullable enable
namespace Web.ViewModels {
    public class PersonListViewModel {
        public Guid Id { get; set; }

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

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
#nullable disable
#pragma warning disable CS8618
