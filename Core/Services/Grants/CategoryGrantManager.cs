using System.Linq;

using Core.Data.Entities;

using Microsoft.AspNetCore.Http;

namespace Core.Services {
    public class CategoryGrantManager: GrantManager<CategoryEntity> {
        public CategoryGrantManager(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) {
        }

        public override IQueryable<CategoryEntity> Filter(IQueryable<CategoryEntity> query) {
            return query.Where(x => !x.Grants.Any(z => z.UserId == UserId));
        }
    }
}
