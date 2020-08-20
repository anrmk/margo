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
        Task<List<CompanyEntity>> FindAll(bool ignoreFilters = false);
        Task<List<CompanyEntity>> FindAll(Guid[] ids);
    }

    public class CompanyManager: AsyncEntityManager<CompanyEntity>, ICompanyManager {
        private readonly GrantManager<CompanyEntity> _grantManager;

        public CompanyManager(IApplicationContext context, GrantManager<CompanyEntity> grantManager) : base(context) {
            _grantManager = grantManager;
        }

        public async Task<CompanyEntity> FindInclude(Guid id) {
            return await _grantManager.Filter(DbSet)
                .Include(x => x.Sections)
                    .ThenInclude(x => x.Fields)
                .Include(x => x.Data)
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<List<CompanyEntity>> FindAll(bool ignoreFilters) {
            return await (ignoreFilters
                    ? DbSet.AsQueryable()
                    : _grantManager.Filter(DbSet))
                .ToListAsync();
        }

        public async Task<List<CompanyEntity>> FindAll(Guid[] ids) {
            return await _grantManager.Filter(DbSet)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
