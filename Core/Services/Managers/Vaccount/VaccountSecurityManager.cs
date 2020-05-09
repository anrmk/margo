using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {

    public interface IVaccountSecurityManager: IEntityManager<VaccountSecurityEntity> {
        Task<VaccountSecurityEntity> FindInclude(long id);
    }

    public class VaccountSecurityManager: AsyncEntityManager<VaccountSecurityEntity>, IVaccountSecurityManager {
        public VaccountSecurityManager(IApplicationContext context) : base(context) { }

        public async Task<VaccountSecurityEntity> FindInclude(long id) {
            return await DbSet
                .Include(x => x.SecurityQuestions)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
