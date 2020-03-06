using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class BulkInvoiceViewModel {
        public string Header { get; set; }

        [Display(Name = "Date from")]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; } = DateTime.Now;

        [Display(Name = "Date to")]
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; } = DateTime.Now.AddDays(30);

        [Display(Name = "Company")]
        public long? CompanyId { get; set; }
        //public CompanyViewModel Company { get; set; }

        [Display(Name = "Customers")]
        public List<long> Customers { get; set; }

        [Display(Name = "Summary Range")]
        public long? SummaryRangeId { get; set; }

        public decimal Balance { get; set; }

        public virtual List<InvoiceViewModel> Invoices { get; set; }
    }
}
