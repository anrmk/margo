using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities.Vaccount;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IVaccountSecurityQuestionManager: IEntityManager<VaccountSecurityQuestionEntity> {
        Task<List<VaccountSecurityQuestionEntity>> FindBySecurityId(long id);
    }

    public class VaccountSecurityQuestionManager: AsyncEntityManager<VaccountSecurityQuestionEntity>, IVaccountSecurityQuestionManager {
        public VaccountSecurityQuestionManager(IApplicationContext context) : base(context) { }

        public async Task<List<VaccountSecurityQuestionEntity>> FindBySecurityId(long id) {
            return await DbSet.Where(x => x.SecurityId == id).ToListAsync();
        }
    }
}
