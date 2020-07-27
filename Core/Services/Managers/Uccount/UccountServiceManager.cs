using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Managers
{
    public interface IUccountServiceManager : IEntityManager<UccountServiceEntity>
    {
        Task<List<UccountServiceEntity>> FindAll(long accountId);
    }

    public class UccountServiceManager : AsyncEntityManager<UccountServiceEntity>, IUccountServiceManager
    {
        public UccountServiceManager(IApplicationContext context) : base(context) { }

        public async Task<List<UccountServiceEntity>> FindAll(long accountId)
        {
            return await DbSet
                .Where(x => x.Account.Id == accountId)
                .ToListAsync();
        }
    }
}
