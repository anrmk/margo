using System.Linq;
using Core.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Services.Grants {
    public class UccountGrantService: GrantService<UccountEntity> {
        public UccountGrantService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) {
        }

        public override IQueryable<UccountEntity> Filter(IQueryable<UccountEntity> query) {
            return query.Where(x => !x.Company.Grants.Any(z => z.UserId == UserId)
                || x.Company.Grants.SingleOrDefault(z => z.UserId == UserId).IsGranted);
        }
    }
}
