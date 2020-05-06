using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface ICompanyAddressManager: IEntityManager<CompanyAddressEntity> {
    }

    public class CompanyAddressManager: AsyncEntityManager<CompanyAddressEntity>, ICompanyAddressManager {
        public CompanyAddressManager(IApplicationContext context) : base(context) { }
    }
}
