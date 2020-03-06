using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class InvoiceListViewModel {
        public long Id { get; set; }
        public string No { get; set; }
        public double Subtotal { get; set; }
        public double TaxRate { get; set; }
        public string Amount => (Subtotal * (1 + TaxRate / 100)).ToString("0.##");

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string DueDate { get; set; }

        public double PaymentAmount { get; set; }
        public string PaymentDate { get; set; }

        public long CompanyId { get; set; }
        public string CompanyName { get; set; }

        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
