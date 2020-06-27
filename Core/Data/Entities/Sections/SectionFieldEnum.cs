using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Entities {
    public enum SectionFieldEnum {
        [Display(Name ="String")]
        STRING,
        [Display(Name = "Number")]
        NUMBER,
        [Display(Name = "Date Time")]
        DATE
    }
}
