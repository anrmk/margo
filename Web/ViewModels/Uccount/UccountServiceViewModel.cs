using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class UccountServiceViewModel {
        public Guid Id { get; set; }
        [Display(Name = "Uccount")]
        public Guid? UccountId { get; set; }
        public string Name { get; set; }
        public Guid? CategoryId { get; set; }

        public IEnumerable<UccountServiceFieldViewModel> Fields { get; set; }
    }
}
