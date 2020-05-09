using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class VaccountSecurityQuestionViewModel {
        public long Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Question { get; set; }

        [Required]
        [MaxLength(512)]
        public string Answer { get; set; }

        public long? SecurityId { get; set; }
    }
}
