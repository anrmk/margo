using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IVendorManager: IEntityManager<VendorEntity> {
        Task<VendorEntity> FindInclude(long id);
        Task<List<VendorEntity>> FindAll();
    }

    public class VendorManager: AsyncEntityManager<VendorEntity>, IVendorManager {
        public VendorManager(IApplicationContext context) : base(context) { }

        public async Task<VendorEntity> FindInclude(long id) {
            return await DbSet
                .Include(x => x.Address)
               .Where(x => x.Id == id)
               .FirstOrDefaultAsync();
        }

        public async Task<List<VendorEntity>> FindAll() {
            return await DbSet
                .Include(x => x.Address)
                .ToListAsync();
        }
    }
}
