using System;
using System.Collections.Generic;

namespace Web.ViewModels {
    public class AppNetUserListViewModel {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
        public bool IsLocked => LockoutEnd.HasValue;

        public List<string> Roles { get; set; }

        public long? ProfileId { get; set; }
    }
}
