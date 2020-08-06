
using System;

using Core.Extension;

namespace Core.Data.Dto {
    public class InvoiceFilterDto: PagerFilter {
        public long? PersonId { get; set; }
        public long? CompanyId { get; set; }
        public long? VendorId { get; set; }
        public DateTime? Date { get; set; }
        public bool Unpaid { get; set; }
    }
}
