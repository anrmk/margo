using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities.Vendor;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IVendorMediaManager: IEntityManager<VendorMediaEntity> {
        Task<List<VendorMediaEntity>> FindByVendorId(long id);
    }

    public class VendorMediaManager: AsyncEntityManager<VendorMediaEntity>, IVendorMediaManager {
        public VendorMediaManager(IApplicationContext context) : base(context) { }

        public async Task<List<VendorMediaEntity>> FindByVendorId(long id) {
            return await DbSet.Where(x => x.VendorId == id).ToListAsync();
        }
    }
}