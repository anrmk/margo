using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Data.Entities;
using Core.Extension;
using Core.Services.Managers;
using Microsoft.AspNetCore.Http;

namespace Core.Services {
    public class UccountServiceGrantManager {

        private readonly string UserId;
        private readonly IUserCategoryGrantsManager _userCategoryGrantsManager;

        public UccountServiceGrantManager(IHttpContextAccessor httpContextAccessor, IUserCategoryGrantsManager userCategoryGrantsManager) {
            UserId = httpContextAccessor.GetUserId();
            _userCategoryGrantsManager = userCategoryGrantsManager;
        }

        public async Task FilterByUser(UccountEntity uccount) {
            var deniedCategoryIds = await GetDeniedCategoryIdList();
            uccount.Services = Filter(uccount.Services, deniedCategoryIds);
        }

        public async Task FilterByUser(List<UccountEntity> uccountCollection) {
            var deniedCategoryIds = await GetDeniedCategoryIdList();
            uccountCollection.ForEach(x => {
                x.Services = Filter(x.Services, deniedCategoryIds);
            });
        }

        private ICollection<UccountServiceEntity> Filter(ICollection<UccountServiceEntity> collection, HashSet<Guid> deniedCategoryIdSet) {
            return collection.Where(z => !z.CategoryId.HasValue || !deniedCategoryIdSet.Contains(z.CategoryId.Value)).ToList();
        }

        private async Task<HashSet<Guid>> GetDeniedCategoryIdList() {
            var cateoryGrantList = await _userCategoryGrantsManager.FindByUserId(UserId);
            return cateoryGrantList.Select(x => x.CategoryId).ToHashSet();
        }
    }
}
