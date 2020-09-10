using System;
using System.ComponentModel.DataAnnotations;
using Core.Data.Enums;

namespace Web.ViewModels {
    public class TodoListViewModel {
        public Guid Id { get; set; }

        [Display(Name = "Message")]
        public string Description { get; set; }

        [Display(Name = "Status")]
        public bool IsCompleted { get; set; }

        public TodoPriorityEnum Priority { get; set; }

        [Display(Name = "User")]
        public string UserName { get; set; }

        [Display(Name = "Added date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified")]
        public DateTime UpdatedDate { get; set; }

        [Display(Name = "Added by")]
        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
    }
}
