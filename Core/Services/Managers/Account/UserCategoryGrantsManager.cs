using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IUserCategoryGrantsManager: IEntityManager<AspNetUserGrantEntity> {
    }

    public class UserCategoryGrantsManager: AsyncEntityManager<AspNetUserGrantEntity>, IUserCategoryGrantsManager {
        public UserCategoryGrantsManager(IApplicationContext context) : base(context) { }
    }
}
