using System;
using System.Collections.Generic;
using System.Text;
using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IUccountVendorFieldManager: IEntityManager<UccountVendorFieldEntity> {
    }

    public class UccountVendorFieldManager: AsyncEntityManager<UccountVendorFieldEntity>, IUccountVendorFieldManager {
        public UccountVendorFieldManager(IApplicationContext context) : base(context) { }
    }
}
