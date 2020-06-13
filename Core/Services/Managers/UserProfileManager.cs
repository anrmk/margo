using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IUserProfileManager: IEntityManager<AppNetUserProfileEntity> {
    }

    public class UserProfileManager: AsyncEntityManager<AppNetUserProfileEntity>, IUserProfileManager {
        public UserProfileManager(IApplicationContext context) : base(context) { }
    }
}
