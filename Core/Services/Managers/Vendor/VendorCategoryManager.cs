﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IVendorCategoryManager: IEntityManager<VendorCategoryEntity> {
        Task<List<VendorCategoryEntity>> FindAll(Guid vendorId);
        Task<VendorCategoryEntity> FindInclude(Guid id);
    }

    public class VendorCategoryManager: AsyncEntityManager<VendorCategoryEntity>, IVendorCategoryManager {
        public VendorCategoryManager(IApplicationContext context) : base(context) { }

        public async Task<List<VendorCategoryEntity>> FindAll(Guid vendorId) {
            return await DbSet
                .Include(x => x.Category)
                .Where(x => x.VendorId.Equals(vendorId))
                .ToListAsync();
        }

        public async Task<VendorCategoryEntity> FindInclude(Guid id) {
            return await DbSet
                .Include(x => x.Category)
                    .ThenInclude(x => x.Fields)
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}