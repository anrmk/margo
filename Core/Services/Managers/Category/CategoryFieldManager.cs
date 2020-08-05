using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ICategoryFieldManager: IEntityManager<CategoryFieldEntity> {
        Task<List<CategoryFieldEntity>> FindAll(long categoryId);
        Task<List<CategoryFieldEntity>> FindAll(long[] ids);
    }

    public class CategoryFieldManager: AsyncEntityManager<CategoryFieldEntity>, ICategoryFieldManager {
        public CategoryFieldManager(IApplicationContext context) : base(context) { }

        public async Task<List<CategoryFieldEntity>> FindAll(long categoryId) {
            return await DbSet
                .Where(x => x.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<List<CategoryFieldEntity>> FindAll(long[] ids) {
            return await DbSet
            .Include(x => x.Category)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
        }
    }
}
