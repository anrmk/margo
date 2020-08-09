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
        Task<List<CategoryEntity>> FindAll();
        Task<List<CategoryEntity>> FindAll(Guid[] ids);
    }

    public class CategoryManager: AsyncEntityManager<CategoryEntity>, ICategoryManager {
        public CategoryManager(IApplicationContext context) : base(context) { }

        public async Task<CategoryEntity> FindInclude(Guid id) {
            return await DbSet
                .Include(x => x.Parent)
                .Include(x => x.Fields)
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<List<CategoryEntity>> FindAll() {
            return await DbSet
                .Include(x => x.Fields)
                .ToListAsync();
        }

        public async Task<List<CategoryEntity>> FindAll(Guid[] ids) {
            return await DbSet
            .Include(x => x.Fields)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
        }
    }
}
