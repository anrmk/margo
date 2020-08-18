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
    public interface ICategoryManager: IEntityManager<CategoryEntity> {
        Task<CategoryEntity> FindInclude(Guid id);
        Task<List<CategoryEntity>> FindAll(bool ignoreQueryFilters = false);
        Task<List<CategoryEntity>> FindAll(Guid[] ids);
        Task<List<AspNetUserGrantEntity>> FindAllGrantsByUser(string userId);
    }

    public class CategoryManager: AsyncEntityManager<CategoryEntity>, ICategoryManager {
        private readonly GrantService<CategoryEntity> _grantService;

        public CategoryManager(IApplicationContext context, GrantService<CategoryEntity> grantService) : base(context) {
            _grantService = grantService;
        }

        public async Task<CategoryEntity> FindInclude(Guid id) {
            return await _grantService.Filter(DbSet)
                .Include(x => x.Parent)
                .Include(x => x.Fields)
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<List<CategoryEntity>> FindAll(bool ignoreQueryFilters) {
            var query = DbSet.Include(x => x.Fields);

            return await (ignoreQueryFilters
                ? query
                : _grantService.Filter(query)).ToListAsync();
        }

        public async Task<List<CategoryEntity>> FindAll(Guid[] ids) {
            return await _grantService.Filter(DbSet)
            .Include(x => x.Fields)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
        }

        public async Task<List<AspNetUserGrantEntity>> FindAllGrantsByUser(string userId) {
            return await DbSet
                .Include(x => x.Grants)
                .Select(x => !x.Grants.Any(z => z.UserId == userId)
                    ? new AspNetUserGrantEntity {
                        CategoryId = x.Id,
                        Category = x,
                        IsGranted = true,
                        UserId = userId
                    }
                    : x.Grants.SingleOrDefault(z => z.UserId == userId))
                .IgnoreQueryFilters()
                .ToListAsync();
        }
    }
}
