using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ICompanyManager: IEntityManager<CompanyEntity> {
        Task<CompanyEntity> FindInclude(Guid id);
        Task<List<CompanyEntity>> FindAll();
        Task<List<CompanyEntity>> FindAll(Guid[] ids);
    }

    public class CompanyManager: AsyncEntityManager<CompanyEntity>, ICompanyManager {
        public CompanyManager(IApplicationContext context) : base(context) { }

        public async Task<CompanyEntity> FindInclude(Guid id) {
            return await DbSet
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<List<CompanyEntity>> FindAll() {
            return await DbSet
                .ToListAsync();
        }

        public async Task<List<CompanyEntity>> FindAll(Guid[] ids) {
            return await DbSet
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
