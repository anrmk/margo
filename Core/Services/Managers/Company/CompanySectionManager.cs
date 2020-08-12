using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ICompanySectionManager: IEntityManager<CompanySectionEntity> {
        Task<CompanySectionEntity> FindInclude(Guid id);
        Task<List<CompanySectionEntity>> FindAll(Guid id);
    }

    public class CompanySectionManager: AsyncEntityManager<CompanySectionEntity>, ICompanySectionManager {
        public CompanySectionManager(IApplicationContext context) : base(context) { }
        public async Task<CompanySectionEntity> FindInclude(Guid id) {
            return await DbSet
                .Include(x => x.Fields)
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<List<CompanySectionEntity>> FindAll(Guid id) {
            return await DbSet
                .Include(x => x.Fields)
                .Where(x => x.Company.Id.Equals(id))
                .ToListAsync();
        }
    }
}
