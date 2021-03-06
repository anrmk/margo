﻿using System;
using System.ComponentModel.DataAnnotations;

using Core.Data;
using Core.Extension;

namespace Web.ViewModels {
    public class PaymentViewModel {
        public long Id { get; set; }

        [Display(Name = "Reference no.")]
        public string No { get; set; }

        [Display(Name = "Payment date")]
        public DateTime Date { get; set; }

        [Display(Name = "Amount received")]
        public decimal Amount { get; set; }

        [Display(Name = "Payment method")]
        public PaymentMethodEnum Method { get; set; }

        [Display(Name = "Payment method")]
        public string MethodName {
            get {
                var value = Method.GetAttribute<DisplayAttribute>();
                return value.Name;
            }
        }

        [Display(Name = "Memo")]
        public string Note { get; set; }

        [Display(Name = "Invoice")]
        public long? InvoiceId { get; set; }

        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }
    }
}
