
using System;

namespace Core.Data.Dto {
    public class InvoiceFilterDto: PagerFilterDto {
        public Guid? CustomerId { get; set; }
        public Guid? VendorId { get; set; }
        public DateTime? Date { get; set; }
        public bool Unpaid { get; set; }
    }
}
