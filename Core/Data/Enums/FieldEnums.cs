using System.ComponentModel.DataAnnotations;

namespace Core.Data.Enums {
    public enum FieldEnum {
        [Display(Name = "String")]
        STRING,
        [Display(Name = "Number")]
        NUMBER,
        [Display(Name = "Date Time")]
        DATE
    }
}
