using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class AspNetUserGrantsViewModel {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public Guid? EntityId { get; set; }

        public string EntityName { get; set; }

        [Display(Name = "Access")]
        public string IsGranted { get; set; }
    }
}
