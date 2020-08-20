using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class AspNetUserDenyAccessUpdateViewModel {
        [Display(Name = "User")]
        public string UserId { get; set; }
        
        public IEnumerable<Guid> Ids { get; set; }
    }
}
