using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class SectionViewModel {
        public long Id { get; set; }

        [Required]
        [MaxLength(24)]
        public string Name { get; set; }

        public string Code { get; set; }

        [MaxLength(2048)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public bool IsDefault { get; set; }
    }
}
