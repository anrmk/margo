using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IUserCategoryGrantsManager: IEntityManager<AspNetUserCategoryGrantEntity> {
        Task<IEnumerable<AspNetUserCategoryGrantEntity>> FindByUserId(string userId);
    }

    public class UserCategoryGrantsManager: AsyncEntityManager<AspNetUserCategoryGrantEntity>, IUserCategoryGrantsManager {
        public UserCategoryGrantsManager(IApplicationContext context) : base(context) { }

        public async Task<IEnumerable<AspNetUserCategoryGrantEntity>> FindByUserId(string userId) {
            return await DbSet
                .Include(x => x.Category)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
