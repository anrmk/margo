using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface ISectionManager: IEntityManager<SectionEntity> {

    }

    public class SectionManager: AsyncEntityManager<SectionEntity>, ISectionManager {
        public SectionManager(IApplicationContext context) : base(context) { }
    }
}
