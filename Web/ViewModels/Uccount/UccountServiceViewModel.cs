using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class UccountServiceViewModel {
        public long Id { get; set; }
        [Display(Name = "Uccount")]
        public long? UccountId { get; set; }
        [Display(Name = "Category")]
        public long Group { get; set; }
        public IEnumerable<UccountServiceFieldViewModel> Fields { get; set; }
    }
}
