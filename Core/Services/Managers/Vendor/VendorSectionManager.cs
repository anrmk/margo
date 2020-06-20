using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IVendorSectionManager: IEntityManager<VendorSectionEntity> {
        Task<VendorSectionEntity> FindInclude(long id);
        Task<List<VendorSectionEntity>> FindAll();
        Task<List<VendorSectionEntity>> FindAll(long vendorId);
    }

    public class VendorSectionManager: AsyncEntityManager<VendorSectionEntity>, IVendorSectionManager {
        public VendorSectionManager(IApplicationContext context) : base(context) { }

        public async Task<VendorSectionEntity> FindInclude(long id) {
            return await DbSet
                .Include(x => x.Vendor)
                .Include(x => x.Section)
                .Include(x => x.Fields)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<VendorSectionEntity>> FindAll() {
            return await DbSet
                .Include(x => x.Vendor)
                .Include(x => x.Section)
                .Include(x => x.Fields)
                .ToListAsync();
        }

        public async Task<List<VendorSectionEntity>> FindAll(long vendorId) {
            return await DbSet
                .Include(x => x.Vendor)
                .Include(x => x.Section)
                .Include(x => x.Fields)
                .Where(x => x.VendorId == vendorId)
                .OrderBy(x => x.Section.Sort)
                .ToListAsync();
        }
    }
}
