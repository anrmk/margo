using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IInvoiceManager: IEntityManager<InvoiceEntity> {
        Task<InvoiceEntity> FindInclude(long id);
        Task<List<InvoiceEntity>> FindAll();
    }

    public class InvoiceManager: AsyncEntityManager<InvoiceEntity>, IInvoiceManager {
        public InvoiceManager(IApplicationContext context) : base(context) { }
        public async Task<InvoiceEntity> FindInclude(long id) {
            return await DbSet
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<InvoiceEntity>> FindAll() {
            return await DbSet.ToListAsync();
        }
    }
}
