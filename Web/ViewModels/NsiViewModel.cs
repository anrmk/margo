using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class NsiViewModel {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
