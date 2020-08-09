using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IVendorManager: IEntityManager<VendorEntity> {
        Task<VendorEntity> FindInclude(Guid id);
        Task<List<VendorEntity>> FindAll();
        Task<List<VendorEntity>> FindAll(Guid[] ids);
    }

    public class VendorManager: AsyncEntityManager<VendorEntity>, IVendorManager {
        public VendorManager(IApplicationContext context) : base(context) { }

        public async Task<VendorEntity> FindInclude(Guid id) {
            return await DbSet
                .Include(x => x.Fields)
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<List<VendorEntity>> FindAll() {
            return await DbSet
                .Include(x => x.Fields)
                .ToListAsync();
        }

        public async Task<List<VendorEntity>> FindAll(Guid[] ids) {
            return await DbSet
                .Include(x => x.Fields)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
