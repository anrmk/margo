using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface ICompanyAddressMananger: IEntityManager<CompanyAddressEntity> {
    }
    public class CompanyAddressManager: AsyncEntityManager<CompanyAddressEntity>, ICompanyAddressMananger {
        public CompanyAddressManager(IApplicationContext context) : base(context) { }
    }
}
