using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IPaymentManager: IEntityManager<PaymentEntity> {
        Task<PaymentEntity> FindInclude(Guid id);
        Task<List<PaymentEntity>> FindAllByInvoiceId(Guid id);
        Task<List<PaymentEntity>> FindByIds(Guid[] ids);
    }

    public class PaymentManager: AsyncEntityManager<PaymentEntity>, IPaymentManager {
        public PaymentManager(IApplicationContext context) : base(context) { }
        public async Task<PaymentEntity> FindInclude(Guid id) {
            return await DbSet
                .Include(x => x.Invoice)
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<List<PaymentEntity>> FindAllByInvoiceId(Guid id) {
            return await DbSet
                .Include(x => x.Invoice)
                .Where(x => x.InvoiceId.Equals(id))
                .ToListAsync();
        }

        public async Task<List<PaymentEntity>> FindByIds(Guid[] ids) {
            return await DbSet
                .Include(x => x.Invoice)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
