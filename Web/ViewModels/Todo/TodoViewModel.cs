using Core.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class TodoViewModel {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Message")]
        public string Description { get; set; }

        [Range(1, 3)]
        public TodoPriorityEnum Priority { get; set; }

        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; }


        [Required]
        [Display(Name = "Assign to")]
        public string UserId { get; set; }
        public string UserName { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
