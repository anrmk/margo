﻿using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels {
    public class InvoiceFilterViewModel: PagerFilterViewModel {

        [FromQuery(Name = "customer")]
        [Display(Name = "Customer")]
        public Guid? CustomerId { get; set; }

        [FromQuery(Name = "isPerson")]
        public bool IsPerson { get; set; }

        [FromQuery(Name = "vendor")]
        [Display(Name = "Vendor")]
        public Guid? VendorId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [FromQuery(Name = "unpaid")]
        [Display(Name = "Unpaid")]
        public bool Unpaid { get; set; }
    }
}
