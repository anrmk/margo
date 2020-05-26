using System;

namespace Web.ViewModels {
    public class InvoiceListViewModel {
        public long Id { get; set; }
        public string No { get; set; }
        public string Amount { get; set; }
        public double TaxRate { get; set; }

        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }

        public long CompanyId { get; set; }
        public string CompanyName { get; set; }

        public long VendorId { get; set; }
        public string VendorName { get; set; }

        public bool IsDraft { get; set; }

        public bool IsPayd { get; private set; }
        public decimal? PaymentAmount { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public string BalanceAmount { get; private set; }
    }
}
