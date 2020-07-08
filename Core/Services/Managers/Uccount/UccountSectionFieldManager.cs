using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IUccountSectionFieldManager: IEntityManager<UccountSectionFieldEntity> {
        Task<List<UccountSectionFieldEntity>> FindAll(long sectionId);
    }

    public class UccountSectionFieldManager: AsyncEntityManager<UccountSectionFieldEntity>, IUccountSectionFieldManager {
        public UccountSectionFieldManager(IApplicationContext context) : base(context) { }

        public async Task<List<UccountSectionFieldEntity>> FindAll(long sectionId) {
            return await DbSet
                .Where(x => x.SectionId == sectionId)
                .ToListAsync();
        }
    }
}
