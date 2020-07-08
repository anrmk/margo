using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ISectionManager: IEntityManager<SectionEntity> {
        Task<SectionEntity> FindInclude(long id);
        Task<List<SectionEntity>> FindAll(long[] ids);
    }

    public class SectionManager: AsyncEntityManager<SectionEntity>, ISectionManager {
        public SectionManager(IApplicationContext context) : base(context) { }

        public async Task<SectionEntity> FindInclude(long id) {
            return await DbSet
                .Include(x => x.Fields)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<SectionEntity>> FindAll(long[] ids) {
            return await DbSet
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
