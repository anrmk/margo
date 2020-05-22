using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class LogFilterViewModel: PagerFilterViewModel {
        [Display(Name = "User")]
        public string UserName { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
    }
}
