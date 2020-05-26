using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IPaymentManager: IEntityManager<PaymentEntity> {
        Task<PaymentEntity> FindInclude(long id);
        Task<List<PaymentEntity>> FindAllByInvoiceId(long id);
    }

    public class PaymentManager: AsyncEntityManager<PaymentEntity>, IPaymentManager {
        public PaymentManager(IApplicationContext context) : base(context) { }
        public async Task<PaymentEntity> FindInclude(long id) {
            return await DbSet
                .Include(x => x.Invoice)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<PaymentEntity>> FindAllByInvoiceId(long id) {
            return await DbSet
                .Include(x => x.Invoice)
                .Where(x => x.InvoiceId == id)
                .ToListAsync();
        }
    }
}
