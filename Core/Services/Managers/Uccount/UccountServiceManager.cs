using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IUccountServiceManager: IEntityManager<UccountServiceEntity> {
        Task<UccountServiceEntity> FindInclude(long id);
        Task<List<UccountServiceEntity>> FindAll(long accountId);
    }

    public class UccountServiceManager: AsyncEntityManager<UccountServiceEntity>, IUccountServiceManager {
        public UccountServiceManager(IApplicationContext context) : base(context) { }
        public async Task<UccountServiceEntity> FindInclude(long id) {
            return await DbSet
                .Include(x => x.Fields)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<UccountServiceEntity>> FindAll(long id) {
            return await DbSet
                .Include(x => x.Fields)
                .Where(x => x.Account.Id == id)
                .ToListAsync();
        }
    }
}
