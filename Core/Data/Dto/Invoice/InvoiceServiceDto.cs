using System;

namespace Core.Data.Dto {
    public class InvoiceServiceDto {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Link for grouping by type
        /// </summary>
        public Guid CategoryId { get; set; }

        public Guid InvoiceId { get; set; }
    }
}
