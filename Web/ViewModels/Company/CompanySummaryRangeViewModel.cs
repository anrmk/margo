using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class CompanySummaryRangeViewModel {
        public long CompanyId { get; set; }

        [Display(Name = "From")]
        [Range(0, 999999)]
        public decimal From { get; set; }

        [Display(Name = "To")]
        [Range(0, 999999)]
        public decimal To { get; set; }
    }
}
