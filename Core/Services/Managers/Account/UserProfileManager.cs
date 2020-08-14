using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IUserProfileManager: IEntityManager<AspNetUserProfileEntity> {
    }

    public class UserProfileManager: AsyncEntityManager<AspNetUserProfileEntity>, IUserProfileManager {
        public UserProfileManager(IApplicationContext context) : base(context) { }
    }
}
