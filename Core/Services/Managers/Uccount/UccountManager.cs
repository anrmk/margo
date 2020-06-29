
using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IUccountManager: IEntityManager<UccountEntity> {

    }

    public class UccountManager: AsyncEntityManager<UccountEntity>, IUccountManager {
        public UccountManager(IApplicationContext context) : base(context) { }
    }
}
