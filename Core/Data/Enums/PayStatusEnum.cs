using System.ComponentModel.DataAnnotations;

namespace Core.Data.Enums {
    public enum PayStatusEnum {
        
        [Display(Name = "Unpaid")]
        Unpaid,
        
        [Display(Name = "Partially paid")]
        PartiallyPaid,
        
        [Display(Name = "Paid")]
        Paid
    }
}
