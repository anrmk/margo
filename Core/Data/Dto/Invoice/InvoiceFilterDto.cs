
using System;

namespace Core.Data.Dto {
    public class InvoiceFilterDto: PagerFilterDto {
        public Guid? PersonId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? VendorId { get; set; }
        public DateTime? Date { get; set; }
        public bool Unpaid { get; set; }
    }
}
