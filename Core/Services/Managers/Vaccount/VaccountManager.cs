using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IVaccountManager: IEntityManager<VaccountEntity> {
        Task<VaccountEntity> FindInclude(long id);
        Task<VaccountEntity> FindBySecurityId(long id);
        Task<List<VaccountEntity>> FindAll();
    }

    public class VaccountManager: AsyncEntityManager<VaccountEntity>, IVaccountManager {
        public VaccountManager(IApplicationContext context) : base(context) { }

        public async Task<List<VaccountEntity>> FindAll() {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Vendor)
                .ToListAsync();
        }

        public async Task<VaccountEntity> FindInclude(long id) {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Vendor)
                .Include(x => x.Security)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<VaccountEntity> FindBySecurityId(long id) {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Vendor)
                .Include(x => x.Security)
                .Where(x => x.SecurityId == id)
                .FirstOrDefaultAsync();
        }
    }
}
