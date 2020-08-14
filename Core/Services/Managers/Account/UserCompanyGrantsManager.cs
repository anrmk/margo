using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IUserCompanyGrantsManager: IEntityManager<AspNetUserGrantEntity> {
    }

    public class UserCompanyGrantsManager: AsyncEntityManager<AspNetUserGrantEntity>, IUserCompanyGrantsManager {
        public UserCompanyGrantsManager(IApplicationContext context) : base(context) { }
    }
}
