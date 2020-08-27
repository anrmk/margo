
using System;
using Core.Data.Enums;

namespace Core.Data.Dto {
    public class InvoiceFilterDto: PagerFilterDto {
        public UccountTypes? Kind { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? VendorId { get; set; }
        public DateTime? Date { get; set; }
        public bool Unpaid { get; set; }
    }
}
