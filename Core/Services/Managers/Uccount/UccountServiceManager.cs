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
        Task<UccountServiceEntity> FindIncludePublicData(Guid id);
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

        public async Task<UccountServiceEntity> FindIncludePublicData(Guid id) {
            return await DbSet
                .Select(x => new UccountServiceEntity {
                    Id = x.Id,
                    Name = x.Name,
                    AccountId = x.AccountId,
                    Fields = x.Fields.Where(z => !z.IsHidden).ToList()
                })
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
