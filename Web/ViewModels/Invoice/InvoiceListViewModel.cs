using System;

namespace Web.ViewModels {
    public class InvoiceListViewModel {
        public long Id { get; set; }
        public string No { get; set; }
        public string Amount { get; set; }
        public double TaxRate { get; set; }

        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }

        public long AccountId { get; set; }
        public string AccountName { get; set; }

        public string CompanyName { get; set; }
        public string VendorName { get; set; }

        public bool IsDraft { get; set; }

        public double PaymentAmount { get; set; }
        public string PaymentDate { get; set; }
    }
}
