using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ICompanyManager: IEntityManager<CompanyEntity> {
        Task<CompanyEntity> FindInclude(long id);
        Task<List<CompanyEntity>> FindAll();
        Task<List<CompanyEntity>> FindAll(long[] ids);
    }

    public class CompanyManager: AsyncEntityManager<CompanyEntity>, ICompanyManager {
        public CompanyManager(IApplicationContext context) : base(context) { }

        public async Task<CompanyEntity> FindInclude(long id) {
            return await DbSet
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<CompanyEntity>> FindAll() {
            return await DbSet
                .ToListAsync();
        }

        public async Task<List<CompanyEntity>> FindAll(long[] ids) {
            return await DbSet
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
