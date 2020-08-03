using System;
using System.Collections;
using System.Collections.Generic;
using Core.Data.Entities;

namespace Core.Data.Dto {
    public class InvoiceDto {
        public long Id { get; set; }
        public string No { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }

        public bool IsPayd { get; private set; }
        public decimal? PaymentAmount { get; private set; }
        public DateTime? PaymentDate { get; private set; }

        public bool IsDraft { get; set; }

        public string CustomerName { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public long AccountId { get; set; }
        public UccountDto Account { get; set; }

        public IEnumerable<InvoiceServiceDto> Services { get; set; }
    }
}