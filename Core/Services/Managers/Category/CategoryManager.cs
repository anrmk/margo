using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ICategoryManager: IEntityManager<CategoryEntity> {
        Task<CategoryEntity> FindInclude(Guid id);
        Task<List<CategoryEntity>> FindAll(bool ignoreFilters = false);
        Task<List<CategoryEntity>> FindAll(Guid[] ids);
    }

    public class CategoryManager: AsyncEntityManager<CategoryEntity>, ICategoryManager {
        private readonly GrantManager<CategoryEntity> _grantManager;

        public CategoryManager(IApplicationContext context, GrantManager<CategoryEntity> grantManager) : base(context) {
            _grantManager = grantManager;
        }

        public async Task<CategoryEntity> FindInclude(Guid id) {
            return await _grantManager.Filter(DbSet)
                .Include(x => x.Parent)
                .Include(x => x.Fields)
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<List<CategoryEntity>> FindAll(bool ignoreFilters) {
            return await (ignoreFilters
                    ? DbSet.AsQueryable()
                    : _grantManager.Filter(DbSet))
                .Include(x => x.Fields)
                .ToListAsync();
        }

        public async Task<List<CategoryEntity>> FindAll(Guid[] ids) {
            return await _grantManager.Filter(DbSet)
                .Include(x => x.Fields)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
