using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IUccountSectionManager: IEntityManager<UccountSectionEntity> {

    }

    public class UccountSectionManager: AsyncEntityManager<UccountSectionEntity>, IUccountSectionManager {
        public UccountSectionManager(IApplicationContext context) : base(context) { }
    }
}
