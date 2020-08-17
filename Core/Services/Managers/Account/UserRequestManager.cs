
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IUserRequestManager: IEntityManager<AspNetUserRequestEntity> {
        Task<AspNetUserRequestEntity> FindInclude(string userName, Guid modelId);
        Task<List<AspNetUserRequestEntity>> FindAll(Guid[] ids);
    }

    public class UserRequestManager: AsyncEntityManager<AspNetUserRequestEntity>, IUserRequestManager {
        public UserRequestManager(IApplicationContext context) : base(context) { }

        public async Task<AspNetUserRequestEntity> FindInclude(string userName, Guid modelId) {
            return await DbSet
                .Where(x => x.CreatedBy.Equals(userName) && x.ModelId.Equals(modelId))
                .FirstOrDefaultAsync();
        }

        public async Task<List<AspNetUserRequestEntity>> FindAll(Guid[] ids) {
            return await DbSet.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
    }
}
