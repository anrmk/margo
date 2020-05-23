using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ILogManager: IEntityManager<LogEntity> {
        Task<List<LogEntity>> FindAllByUserName(string userName);
    }

    public class LogManager: AsyncEntityManager<LogEntity>, ILogManager {
        public LogManager(IApplicationContext context) : base(context) { }

        //public async Task<CompanyEntity> FindAllByUserName(long userName) {
        //    return await DbSet
        //        .Include(x => x.Address)
        //        .Where(x => x.Id == id)
        //        .FirstOrDefaultAsync();
        //}

        public async Task<List<LogEntity>> FindAllByUserName(string userName) {
            return await DbSet
                .Where(x => x.UserName.Equals(userName))
                .ToListAsync();
        }
    }
}
