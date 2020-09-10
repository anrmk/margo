using System;
using System.ComponentModel.DataAnnotations;
using Core.Data.Enums;

namespace Web.ViewModels {
    public class TodoFilterViewModel: PagerFilterViewModel {
        [Display(Name = "Assigned")]
        public TodoUserTypeEnum? Type { get; set; }

        [Display(Name = "Message")]
        public string Text { get; set; }

        [Display(Name = "Priority")]
        public TodoPriorityEnum? Priority { get; set; }

        [Display(Name = "Sorting by")]
        public TodoSortingEnum SortingBy { get; set; }

        [Display(Name = "Completed items")]
        public bool IncludeCompleted { get; set; }

        [Display(Name = "From")]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "To")]
        public DateTime? DateTo { get; set; }
    }
}
