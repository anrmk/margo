using System.ComponentModel.DataAnnotations;

namespace Core.Data.Enums {
    public enum PayStatusEnum {

        [Display(Name = "Unpaid")]
        UNPAID,

        [Display(Name = "Partially paid")]
        PARTIALLY,

        [Display(Name = "Paid")]
        PAID
    }
}
