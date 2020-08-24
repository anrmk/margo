
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IUccountManager: IEntityManager<UccountEntity> {
        Task<UccountEntity> FindInclude(Guid id, bool filter = true);
        Task<List<UccountEntity>> FindAll(Guid[] ids);
        Task<List<UccountEntity>> FindAll();
        Task<List<UccountServiceEntity>> FindByCompany(Guid companyId);
    }

    public class UccountManager: AsyncEntityManager<UccountEntity>, IUccountManager {
        private readonly GrantManager<UccountEntity> _grantManager;

        public UccountManager(IApplicationContext context, GrantManager<UccountEntity> grantManager) : base(context) {
            _grantManager = grantManager;
        }

        public async Task<UccountEntity> FindInclude(Guid id, bool filter) {
            var query = filter
                ? _grantManager.Filter(DbSet)
                : DbSet.AsQueryable();

            return await query
                .Include(x => x.Company)
                .Include(x => x.Vendor)
                .Include(x => x.Services)
                    .ThenInclude(x => x.Fields)
                .Include(x => x.Person)
                .Include(x => x.Fields)
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<List<UccountEntity>> FindAll(Guid[] ids) {
            return await _grantManager.Filter(DbSet)
                .Include(x => x.Company)
                .Include(x => x.Vendor)
                .Include(x => x.Services)
                    .ThenInclude(x => x.Fields)
                .Include(x => x.Person)
                .Include(x => x.Fields)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<List<UccountEntity>> FindAll() {
            return await _grantManager.Filter(DbSet)
                .Include(x => x.Company)
                .Include(x => x.Person)
                .ToListAsync();
        }

        public async Task<List<UccountServiceEntity>> FindByCompany(Guid companyId) {
            return await DbSet
                .Where(x => x.CompanyId == companyId)
                .Include(x => x.Services)
                    .ThenInclude(x => x.Fields)
                .SelectMany(x => x.Services)
                .Select(x => new UccountServiceEntity {
                    Id = x.Id,
                    Name = x.Name,
                    Fields = x.Fields.Where(f => !f.IsHidden).ToList(),
                    CategoryId = x.CategoryId
                })
                .ToListAsync();

        }
    }
}
