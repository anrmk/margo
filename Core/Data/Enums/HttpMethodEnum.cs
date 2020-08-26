using System.ComponentModel.DataAnnotations;

namespace Core.Data.Enums {
    public enum HttpMethodEnum {
        [Display(Name = "Read")]
        GET,
        [Display(Name = "Create")]
        POST,
        [Display(Name = "Update")]
        PUT,
        [Display(Name = "Patch")]
        PATCH,
        [Display(Name = "Delete")]
        DELETE
    }
}
