using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface ISupplierAddressManager: IEntityManager<SupplierAddressEntity> {
    }

    public class SupplierAddressManager: AsyncEntityManager<SupplierAddressEntity>, ISupplierAddressManager {
        public SupplierAddressManager(IApplicationContext context) : base(context) { }
    }
}
