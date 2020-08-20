using System.Linq;
using Core.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Services {
    public class UccountGrantManager: GrantManager<UccountEntity> {
        public UccountGrantManager(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) {
        }

        public override IQueryable<UccountEntity> Filter(IQueryable<UccountEntity> query) {
            return query.Where(x => !x.Company.Grants.Any(z => z.UserId == UserId));
        }
    }
}
