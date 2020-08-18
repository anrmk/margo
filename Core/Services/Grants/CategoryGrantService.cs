using System.Linq;
using Core.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Services.Grants {
    public class CategoryGrantService: GrantService<CategoryEntity> {
        public CategoryGrantService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) {
        }

        public override IQueryable<CategoryEntity> Filter(IQueryable<CategoryEntity> query) {
            return query.Where(x => !x.Grants.Any(z => z.UserId == UserId)
                || x.Grants.SingleOrDefault(z => z.UserId == UserId).IsGranted);
        }
    }
}
