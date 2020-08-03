
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IUccountManager: IEntityManager<UccountEntity> {
        Task<UccountEntity> FindInclude(long id);
        Task<List<UccountEntity>> FindAll(long[] ids);
        Task<List<UccountEntity>> FindAll();
    }

    public class UccountManager: AsyncEntityManager<UccountEntity>, IUccountManager {
        public UccountManager(IApplicationContext context) : base(context) { }

        public async Task<UccountEntity> FindInclude(long id) {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Vendor)
                .Include(x => x.Services)
                    .ThenInclude(x => x.Fields)
                .Include(x => x.Person)
                .Include(x => x.Fields)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<UccountEntity>> FindAll(long[] ids) {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Vendor)
                .Include(x => x.Services)
                    .ThenInclude(x => x.Fields)
                .Include(x => x.Person)
                .Include(x => x.Fields)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<List<UccountEntity>> FindAll() {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Person)
                .ToListAsync();
        }
    }
}
