using System.Security.Claims;

using Microsoft.AspNetCore.Http;

namespace Core.Extension {
    public static class ClaimsPrincipalExtension {
        public static string GetUserId(this ClaimsPrincipal principal) {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static string GetUserId(this IHttpContextAccessor httpContextAccessor) {
            return httpContextAccessor.HttpContext.User.GetUserId();
        }
    }
}
