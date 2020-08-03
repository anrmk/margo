using System;
using System.Collections.Generic;

using AutoMapper.Configuration.Annotations;

namespace Core.Data.Dto {
    public class AspNetUserDto {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public List<AspNetRoleDto> Roles { get; set; }
        public long ProfileId { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
