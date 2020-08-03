namespace Web.ViewModels {
    public class InvoiceServiceViewModel {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }

        public long CategoryId { get; set; }
        public long InvoiceId { get; set; }
    }
}
