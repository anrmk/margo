using System;

using Core.Data.Enums;

namespace Core.Data.Dto {
    public class PaymentDto {
        public long Id { get; set; }
        public string No { get; set; }

        public DateTime Date { get; set; }

        public PaymentMethodEnum Method { get; set; }

        public decimal Amount { get; set; }

        public string Note { get; set; }

        public long InvoiceId { get; set; }
        public string InvoiceNo { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
