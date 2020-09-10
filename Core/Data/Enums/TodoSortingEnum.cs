using System.ComponentModel.DataAnnotations;

namespace Core.Data.Enums {
    public enum TodoSortingEnum {
        [Display(Name = "Priority")]
        Priority,

        [Display(Name = "Date")]
        DateCreation
    }
}
