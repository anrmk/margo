﻿using System.Collections.Generic;
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
        Task<List<InvoiceEntity>> FindByIds(long[] ids);
    }

    public class InvoiceManager: AsyncEntityManager<InvoiceEntity>, IInvoiceManager {
        public InvoiceManager(IApplicationContext context) : base(context) { }
        public async Task<InvoiceEntity> FindInclude(long id) {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Vendor)
                .Include(x => x.Payments)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<InvoiceEntity>> FindAll() {
            return await DbSet.ToListAsync();
        }

        public async Task<List<InvoiceEntity>> FindByIds(long[] ids) {
            return await DbSet
                .Include(x => x.Company)
                .Include(x => x.Vendor)
                .Include(x => x.Payments)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
