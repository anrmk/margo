using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IVendorAddressManager: IEntityManager<VendorAddressEntity> {
    }

    public class VendorAddressManager: AsyncEntityManager<VendorAddressEntity>, IVendorAddressManager {
        public VendorAddressManager(IApplicationContext context) : base(context) { }
    }
}
