using System;
using System.Collections.Generic;

namespace Web.ViewModels {
    public class VendorCategoryViewModel {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public virtual List<CategoryFieldViewModel> Fields { get; set; }
    }
}
