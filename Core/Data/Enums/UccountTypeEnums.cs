using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Data.Enums {
    public enum UccountTypes {
        [Display(Name = "Business account")]
        BUSINESS = 0,
        [Display(Name = "Personal account")]
        PERSONAL = 1
    }
}
