using System;

namespace Web.ViewModels {
    public class AspNetUserDenyAccessViewModel {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public Guid? EntityId { get; set; }

        public string EntityName { get; set; }
    }
}
