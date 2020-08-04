using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels {
    public class AspNetUserProfileViewModel {
        public long Id { get; set; }
        //public string Uin { get; set; }
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(64)]
        public string SurName { get; set; }

        [MaxLength(64)]
        public string MiddleName { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

    }
}
