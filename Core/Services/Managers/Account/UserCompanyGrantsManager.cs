using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IUserCompanyGrantsManager: IEntityManager<AspNetUserCompanyGrantEntity> {
        Task<IEnumerable<AspNetUserCompanyGrantEntity>> FindByUserId(string userId);
    }

    public class UserCompanyGrantsManager: AsyncEntityManager<AspNetUserCompanyGrantEntity>, IUserCompanyGrantsManager {
        public UserCompanyGrantsManager(IApplicationContext context) : base(context) { }

        public async Task<IEnumerable<AspNetUserCompanyGrantEntity>> FindByUserId(string userId) {
            return await DbSet
                .Include(x => x.Company)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
