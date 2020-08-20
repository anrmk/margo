using System.Linq;
using Core.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Services {
    public class CompanyGrantManager: GrantManager<CompanyEntity> {
        public CompanyGrantManager(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) {
        }

        public override IQueryable<CompanyEntity> Filter(IQueryable<CompanyEntity> query) {
            return query.Where(x => !x.Grants.Any(z => z.UserId == UserId));
        }
    }
}
