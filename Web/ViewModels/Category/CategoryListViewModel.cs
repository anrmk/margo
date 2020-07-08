﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels {
    public class CategoryListViewModel {
        public long Id { get; set; }
        public string Name { get; set; }

        public long? ParentId { get; set; }
        public string ParentName { get; set; }
    }
}
