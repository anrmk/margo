using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IPersonManager: IEntityManager<PersonEntity> {
        Task<PersonEntity> FindInclude(long id);
        Task<List<PersonEntity>> FindAll();
        Task<List<PersonEntity>> FindAll(long[] ids);
    }

    public class PersonManager: AsyncEntityManager<PersonEntity>, IPersonManager {
        public PersonManager(IApplicationContext context) : base(context) { }

        public async Task<PersonEntity> FindInclude(long id) {
            return await DbSet
                .Include(x => x.Accounts)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<PersonEntity>> FindAll() {
            return await DbSet
                .Include(x => x.Accounts)
                .ToListAsync();
        }

        public async Task<List<PersonEntity>> FindAll(long[] ids) {
            return await DbSet
                .Include(x => x.Accounts)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
