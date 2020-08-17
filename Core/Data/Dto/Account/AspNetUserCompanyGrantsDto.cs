using System;

namespace Core.Data.Dto {
    public class AspNetUserCompanyGrantsDto {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid EntityId { get; set; }
        public CompanyDto Company { get; set; }
        public bool IsGranted { get; set; }
    }
}
