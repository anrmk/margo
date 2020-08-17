using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels {
    public class AspNetUserRequestViewModel {
        public Guid Id { get; set; }

        public Guid ModelId { get; set; }
        public string Model { get; set; }

        [Display(Name ="Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }

    public class AspNetUserRequestListViewModel {
        public Guid Id { get; set; }
        public string ModelTypeName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
