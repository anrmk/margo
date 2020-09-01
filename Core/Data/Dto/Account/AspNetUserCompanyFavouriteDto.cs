using System;

namespace Core.Data.Dto {
    public class AspNetUserCompanyFavouriteDto {
        public long Id { get; set; }

        public string UserId { get; set; }

        public Guid CompanyId { get; set; }
        public CompanyDto Company { get; set; }

        public int Sort { get; set; }
    }
}
