using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class LogFilterViewModel: PagerFilterViewModel {
        public Guid Id { get; set; }

        [Display(Name = "User")]
        public string UserName { get; set; }

        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Message")]
        public new string Search { get; set; }

        public int? Method { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        [Display(Name = "Exceptions")]
        public bool IsException { get; set; }
    }
}
