using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IUccountServiceFieldManager: IEntityManager<UccountServiceFieldEntity> {
        Task<List<UccountServiceFieldEntity>> FindAll(long sectionId);
    }

    public class UccountServiceFieldManager: AsyncEntityManager<UccountServiceFieldEntity>, IUccountServiceFieldManager {
        public UccountServiceFieldManager(IApplicationContext context) : base(context) { }

        public async Task<List<UccountServiceFieldEntity>> FindAll(long sectionId) {
            return await DbSet
                .Where(x => x.ServiceId == sectionId)
                .ToListAsync();
        }
    }
}
