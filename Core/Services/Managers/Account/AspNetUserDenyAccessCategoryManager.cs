using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IAspNetUserDenyAccessCategoryManager: IEntityManager<AspNetUserDenyAccessCategoryEntity> {
        Task<IEnumerable<AspNetUserDenyAccessCategoryEntity>> FindByUserId(string userId);
    }

    public class AspNetUserDenyAccessCategoryManager: AsyncEntityManager<AspNetUserDenyAccessCategoryEntity>, IAspNetUserDenyAccessCategoryManager {
        public AspNetUserDenyAccessCategoryManager(IApplicationContext context) : base(context) { }

        public async Task<IEnumerable<AspNetUserDenyAccessCategoryEntity>> FindByUserId(string userId) {
            return await DbSet
                .Include(x => x.Category)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
