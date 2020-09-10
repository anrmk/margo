using System;
using Core.Data.Enums;

namespace Core.Data.Dto {
    public class TodoFilterDto: PagerFilterDto {
        public TodoUserTypeEnum? Type { get; set; }
        public string Text { get; set; }
        public TodoPriorityEnum? Priority { get; set; }
        public TodoSortingEnum SortingBy { get; set; }
        public bool IncludeCompleted { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string UserId { get; set; }
        public string UserLogin { get; set; }
    }
}
