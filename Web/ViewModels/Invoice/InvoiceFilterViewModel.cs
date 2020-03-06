using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class InvoiceFilterViewModel: PagerFilterViewModel {
        [Display(Name = "Company")]
        public long? CompanyId { get; set; }

        [Display(Name = "Period Date")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
    }
}
