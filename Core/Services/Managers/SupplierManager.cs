using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ISupplierManager: IEntityManager<SupplierEntity> {
        Task<SupplierEntity> FindInclude(long id);
        Task<List<SupplierEntity>> FindAll();
        Task<List<SupplierEntity>> FindAll(long companyId);
    }

    public class SupplierManager: AsyncEntityManager<SupplierEntity>, ISupplierManager {
        public SupplierManager(IApplicationContext context) : base(context) { }

        public async Task<SupplierEntity> FindInclude(long id) {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Address)
               .Where(x => x.Id == id)
               .FirstOrDefaultAsync();
        }

        public async Task<List<SupplierEntity>> FindAll() {
            return await DbSet
                .Include(x => x.Address)
                .ToListAsync();
        }

        public async Task<List<SupplierEntity>> FindAll(long companyId) {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Address)
                .Where(x => x.CompanyId == companyId).ToListAsync();
        }
    }
}
