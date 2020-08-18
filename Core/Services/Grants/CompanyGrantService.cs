using System.Linq;
using Core.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Services.Grants {
    public class CompanyGrantService: GrantService<CompanyEntity> {
        public CompanyGrantService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) {
        }

        public override IQueryable<CompanyEntity> Filter(IQueryable<CompanyEntity> query) {
            return query.Where(x => !x.Grants.Any(z => z.UserId == UserId)
                || x.Grants.SingleOrDefault(z => z.UserId == UserId).IsGranted);
        }
    }
}
