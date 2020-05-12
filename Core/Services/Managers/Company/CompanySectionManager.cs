using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ICompanySectionManager: IEntityManager<CompanySectionEntity> {
        Task<CompanySectionEntity> FindInclude(long id);
        Task<List<CompanySectionEntity>> FindAll(long companyId);
        Task<List<CompanySectionEntity>> FindAll();
    }

    public class CompanySectionManager: AsyncEntityManager<CompanySectionEntity>, ICompanySectionManager {
        public CompanySectionManager(IApplicationContext context) : base(context) { }

        public async Task<CompanySectionEntity> FindInclude(long id) {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Section)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<CompanySectionEntity>> FindAll() {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Section)
                .ToListAsync();
        }

        public async Task<List<CompanySectionEntity>> FindAll(long companyId) {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Section)
                .Include(x => x.Fields)
                .Where(x => x.CompanyId == companyId)
                .ToListAsync();
        }
    }
}
