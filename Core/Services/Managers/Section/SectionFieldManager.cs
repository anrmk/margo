using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {

    public interface ISectionFieldManager: IEntityManager<SectionFieldEntity> {
        Task<List<SectionFieldEntity>> FindAll(long sectionId);
    }

    public class SectionFieldManager: AsyncEntityManager<SectionFieldEntity>, ISectionFieldManager {
        public SectionFieldManager(IApplicationContext context) : base(context) { }

        public async Task<List<SectionFieldEntity>> FindAll(long sectionId) {
            return await DbSet
                .Where(x => x.SectionId == sectionId)
                .ToListAsync();
        }
    }
}
