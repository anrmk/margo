using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IAspNetUserDenyAccessCompanyManager: IEntityManager<AspNetUserDenyAccessCompanyEntity> {
        Task<IEnumerable<AspNetUserDenyAccessCompanyEntity>> FindByUserId(string userId);
    }

    public class AspNetUserDenyAccessCompanyManager: AsyncEntityManager<AspNetUserDenyAccessCompanyEntity>, IAspNetUserDenyAccessCompanyManager {
        public AspNetUserDenyAccessCompanyManager(IApplicationContext context) : base(context) { }

        public async Task<IEnumerable<AspNetUserDenyAccessCompanyEntity>> FindByUserId(string userId) {
            return await DbSet
                .Include(x => x.Company)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
