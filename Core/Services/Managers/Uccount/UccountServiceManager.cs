using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IUccountServiceManager: IEntityManager<UccountServiceEntity> {
        Task<UccountServiceEntity> FindInclude(Guid id);
        Task<List<UccountServiceEntity>> FindAll(Guid accountId);
    }

    public class UccountServiceManager: AsyncEntityManager<UccountServiceEntity>, IUccountServiceManager {
        public UccountServiceManager(IApplicationContext context) : base(context) { }
        public async Task<UccountServiceEntity> FindInclude(Guid id) {
            return await DbSet
                .Include(x => x.Fields)
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<List<UccountServiceEntity>> FindAll(Guid id) {
            return await DbSet
                .Include(x => x.Fields)
                .Where(x => x.Account.Id.Equals(id))
                .ToListAsync();
        }
    }
}
