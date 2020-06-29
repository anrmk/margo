using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IUccountSectionFieldManager: IEntityManager<UccountSectionFieldEntity> {

    }

    public class UccountSectionFieldManager: AsyncEntityManager<UccountSectionFieldEntity>, IUccountSectionFieldManager {
        public UccountSectionFieldManager(IApplicationContext context) : base(context) { }
    }
}
