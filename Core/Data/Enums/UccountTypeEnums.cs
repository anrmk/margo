using System.ComponentModel.DataAnnotations;

namespace Core.Data.Enums {
    public enum UccountTypes {
        [Display(Name = "Business account")]
        BUSINESS = 0,
        [Display(Name = "Personal account")]
        PERSONAL = 1
    }
}
