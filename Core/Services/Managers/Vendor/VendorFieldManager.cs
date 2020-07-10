using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IVendorFieldManager: IEntityManager<VendorFieldEntity> {
        Task<List<VendorFieldEntity>> FindAll(long vendorId);
        Task<List<VendorFieldEntity>> FindAll(long[] ids);
    }

    public class VendorFieldManager: AsyncEntityManager<VendorFieldEntity>, IVendorFieldManager {
        public VendorFieldManager(IApplicationContext context) : base(context) { }

        public async Task<List<VendorFieldEntity>> FindAll(long vendorId) {
            return await DbSet
                .Include(x => x.Vendor)
                .Where(x => x.VendorId == vendorId)
                .ToListAsync();
        }

        public async Task<List<VendorFieldEntity>> FindAll(long[] ids) {
            return await DbSet
                .Include(x => x.Vendor)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
