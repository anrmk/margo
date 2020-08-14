using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class AspNetUserCompanyGrantsViewModel {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Company")]
        public Guid EntityId { get; set; }
        
        [Display(Name = "Name")]
        public string EntityName { get; set; }

        [Display(Name = "Access")]
        public string IsGranted { get; set; }
    }
}
