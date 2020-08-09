using System;

namespace Web.ViewModels {
    public class CategoryListViewModel {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
        public string ParentName { get; set; }
    }
}
