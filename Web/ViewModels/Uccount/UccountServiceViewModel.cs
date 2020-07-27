﻿using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class UccountServiceViewModel {
        public long Id { get; set; }
        [Display(Name = "Uccount")]
        public long UccountId { get; set; }
        [Display(Name = "Category")]
        public long CategoryId { get; set; }
    }
}
