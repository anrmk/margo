using System;

namespace Web.ViewModels {
    public class InvoiceServiceViewModel {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }

        public Guid CategoryId { get; set; }
        public Guid InvoiceId { get; set; }
    }
}
