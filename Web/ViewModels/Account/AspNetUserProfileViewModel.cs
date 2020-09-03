using System.ComponentModel.DataAnnotations;

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

        public string FullName => $"{Name} {SurName} {MiddleName}";

        [MaxLength(2048)]
        public string Description { get; set; }

        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
