using System.ComponentModel.DataAnnotations;

namespace Core.Data.Enums {
    public enum TodoUserTypeEnum {
        [Display(Name = "Created by me")]
        Mine,

        [Display(Name = "Assigned to me from others")]
        ToMe,

        [Display(Name = "Assigned to others by me")]
        FromMe
    }
}
