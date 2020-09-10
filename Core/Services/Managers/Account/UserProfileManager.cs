using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IUserProfileManager: IEntityManager<AspNetUserProfileEntity> {
        Task<AspNetUserProfileEntity> FindInclude(long id);
    }

    public class UserProfileManager: AsyncEntityManager<AspNetUserProfileEntity>, IUserProfileManager {
        public UserProfileManager(IApplicationContext context) : base(context) { }

        public virtual async Task<AspNetUserProfileEntity> FindInclude(long id) {
            return await DbSet.Include(x => x.User).SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
