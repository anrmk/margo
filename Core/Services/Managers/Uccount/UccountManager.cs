
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IUccountManager: IEntityManager<UccountEntity> {
        Task<List<UccountEntity>> FindAll(long[] ids);
    }

    public class UccountManager: AsyncEntityManager<UccountEntity>, IUccountManager {
        public UccountManager(IApplicationContext context) : base(context) { }

        public async Task<List<UccountEntity>> FindAll(long[] ids) {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Vendor)
                .Include(x => x.Services)
                    .ThenInclude(x => x.Fields)
                .Include(x => x.Person)
                .Include(x => x.VendorFields)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
