using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IVendorSectionFieldManager: IEntityManager<VendorSectionFieldEntity> {
        Task<List<VendorSectionFieldEntity>> FindAll(long sectionId);
    }

    public class VendorSectionFieldManager: AsyncEntityManager<VendorSectionFieldEntity>, IVendorSectionFieldManager {
        public VendorSectionFieldManager(IApplicationContext context) : base(context) { }

        public async Task<List<VendorSectionFieldEntity>> FindAll(long sectionId) {
            return await DbSet
                .Where(x => x.SectionId == sectionId)
                .ToListAsync();
        }
    }
}
