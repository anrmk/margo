using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IUserFavouriteCompanyManager: IEntityManager<AspNetUserCompanyFavouriteEntity> {
        Task<List<AspNetUserCompanyFavouriteEntity>> FindByUserId(string userId);
    }

    public class UserFavouriteCompanyManager: AsyncEntityManager<AspNetUserCompanyFavouriteEntity>, IUserFavouriteCompanyManager {
        public UserFavouriteCompanyManager(IApplicationContext context) : base(context) { }

        public async Task<List<AspNetUserCompanyFavouriteEntity>> FindByUserId(string userId) {
            return await DbSet
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Sort)
                .Include(x => x.Company)
                .ToListAsync();
        }
    }
}
