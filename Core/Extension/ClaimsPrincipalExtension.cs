using System.Security.Claims;

namespace Core.Extension {
    public static class ClaimsPrincipalExtension {
        public static string GetUserId(this ClaimsPrincipal principal) {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
