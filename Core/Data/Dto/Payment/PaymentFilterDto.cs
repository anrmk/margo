using System;
using System.Collections.Generic;
using System.Text;
using Core.Extension;

namespace Core.Data.Dto {
    public class PaymentFilterDto: PagerFilter {
        public long? InvoiceId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
