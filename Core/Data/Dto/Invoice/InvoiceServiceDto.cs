using System;

namespace Core.Data.Dto {
    public class InvoiceServiceDto {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Link for grouping by type
        /// </summary>
        public long CategoryId { get; set; }

        public long InvoiceId { get; set; }
    }
}
