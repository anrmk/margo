using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;
using Core.Services.Grants;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ICompanyManager: IEntityManager<CompanyEntity> {
        Task<CompanyEntity> FindInclude(Guid id);
        Task<List<CompanyEntity>> FindAll(bool ignoreQueryFilters = false);
        Task<List<CompanyEntity>> FindAll(Guid[] ids);
        Task<List<AspNetUserGrantEntity>> FindAllGrantsByUser(string userId);
    }

    public class CompanyManager: AsyncEntityManager<CompanyEntity>, ICompanyManager {
        private readonly GrantService<CompanyEntity> _grantService;

        public CompanyManager(IApplicationContext context, GrantService<CompanyEntity> grantService) : base(context) {
            _grantService = grantService;
        }

        public async Task<CompanyEntity> FindInclude(Guid id) {
            return await _grantService.Filter(DbSet)
                .Include(x => x.Sections)
                    .ThenInclude(x => x.Fields)
                .Include(x => x.Data)
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<List<CompanyEntity>> FindAll(bool ignoreQueryFilters) {
            return await (ignoreQueryFilters
                ? DbSet.AsQueryable()
                : _grantService.Filter(DbSet)).ToListAsync();
        }

        public async Task<List<CompanyEntity>> FindAll(Guid[] ids) {
            return await _grantService.Filter(DbSet)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<List<AspNetUserGrantEntity>> FindAllGrantsByUser(string userId) {
            return await DbSet
                .Include(x => x.Grants)
                .Select(x => !x.Grants.Any(z => z.UserId == userId)
                    ? new AspNetUserGrantEntity {
                        CompanyId = x.Id,
                        Company = x,
                        IsGranted = true,
                        UserId = userId
                    }
                    : x.Grants.SingleOrDefault(z => z.UserId == userId))
                .IgnoreQueryFilters()
                .ToListAsync();
        }
    }
}
