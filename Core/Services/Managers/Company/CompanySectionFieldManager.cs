using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ICompanySectionFieldManager: IEntityManager<CompanySectionFieldEntity> {
        Task<List<CompanySectionFieldEntity>> FindAllBySectionId(long id);
    }

    public class CompanySectionFieldManager: AsyncEntityManager<CompanySectionFieldEntity>, ICompanySectionFieldManager {
        public CompanySectionFieldManager(IApplicationContext context) : base(context) { }

        public async Task<List<CompanySectionFieldEntity>> FindAllBySectionId(long id) {
            return await DbSet.Where(x => x.CompanySectionId == id).ToListAsync();
        }
    }
}