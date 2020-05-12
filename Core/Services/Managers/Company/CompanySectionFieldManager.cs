using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface ICompanySectionFieldManager: IEntityManager<CompanySectionFieldEntity> {
        //Task<CompanySectionEntity> FindInclude(long id);
    }

    public class CompanySectionFieldManager: AsyncEntityManager<CompanySectionFieldEntity>, ICompanySectionFieldManager {
        public CompanySectionFieldManager(IApplicationContext context) : base(context) { }

        //public async Task<CompanySectionFieldEntity> FindInclude(long id) {
        //    return await DbSet
        //        .Include(x => x.Section)
        //        .Where(x => x.Id == id)
        //        .FirstOrDefaultAsync();
        //}
    }
}