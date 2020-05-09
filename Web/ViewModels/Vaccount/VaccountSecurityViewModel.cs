using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class VaccountSecurityViewModel {
        public long Id { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        public virtual ICollection<VaccountSecurityQuestionViewModel> SecurityQuestions { get; set; }
    }
}
