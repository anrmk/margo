using System.Linq;

using Core.Extension;

using Microsoft.AspNetCore.Http;

namespace Core.Services {
    public abstract class GrantManager<T> where T : class {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected string UserId => _httpContextAccessor.GetUserId();

        public GrantManager(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        public abstract IQueryable<T> Filter(IQueryable<T> query);
    }
}
