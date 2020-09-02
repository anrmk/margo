using System.ComponentModel.DataAnnotations;

namespace Core.Data.Enums {
    public enum FieldEnum {
        [Display(Name = "String")]
        TEXT,
        [Display(Name = "Number")]
        NUMBER,
        [Display(Name = "Date Time")]
        DATE,
        [Display(Name = "True of False")]
        BOOLEAN,
        [Display(Name = "Password")]
        PASSWORD,
        [Display(Name = "E-mail")]
        EMAIL,
        [Display(Name = "Link")]
        LINK,
        [Display(Name = "List")]
        LIST
    }
}
