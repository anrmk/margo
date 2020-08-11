using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ICompanyDataManager: IEntityManager<CompanyDataEntity> {
        Task<CompanyDataEntity> FindInclude(Guid id);
        Task<List<CompanyDataEntity>> FindAllData(Guid id);
        Task<List<CompanyDataEntity>> FindAll(Guid[] ids);
    }

    public class CompanyDataManager: AsyncEntityManager<CompanyDataEntity>, ICompanyDataManager {
        public CompanyDataManager(IApplicationContext context) : base(context) { }

        public async Task<CompanyDataEntity> FindInclude(Guid id) {
            return await DbSet
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<CompanyDataEntity>> FindAllData(Guid id) {
            return await DbSet
                .Where(x => x.CompanyId == id)
                .Include(x => x.Field)
                .ToListAsync();
        }

        public async Task<List<CompanyDataEntity>> FindAll(Guid[] ids) {
            return await DbSet
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
