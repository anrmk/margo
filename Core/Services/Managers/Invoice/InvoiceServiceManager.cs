using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IInvoiceServiceManager: IEntityManager<InvoiceServiceEntity> {
        Task<List<InvoiceServiceEntity>> FindBy(Guid iguid);
        Task<List<InvoiceServiceEntity>> FindAll(Guid[] ids);

    }

    public class InvoiceServiceManager: AsyncEntityManager<InvoiceServiceEntity>, IInvoiceServiceManager {
        public InvoiceServiceManager(IApplicationContext context) : base(context) { }

        public async Task<List<InvoiceServiceEntity>> FindBy(Guid iguid) {
            return await DbSet
                .Where(x => x.InvoiceId.Equals(iguid))
                .ToListAsync();
        }

        public async Task<List<InvoiceServiceEntity>> FindAll(Guid[] ids) {
            return await DbSet
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
