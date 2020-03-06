using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IUserProfileManager: IEntityManager<ApplicationUserProfileEntity> {
    }

    public class UserProfileManager: AsyncEntityManager<ApplicationUserProfileEntity>, IUserProfileManager {
        public UserProfileManager(IApplicationContext context) : base(context) { }
    }
}
