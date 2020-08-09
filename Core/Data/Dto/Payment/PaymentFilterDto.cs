using System;

namespace Core.Data.Dto {
    public class PaymentFilterDto: PagerFilterDto {
        public Guid? InvoiceId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
