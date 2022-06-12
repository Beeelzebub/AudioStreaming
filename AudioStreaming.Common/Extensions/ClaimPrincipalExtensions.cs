using System.Security.Claims;

namespace AudioStreaming.Common.Extensions
{
    public static class ClaimPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal source) => 
            source.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";

    }
}
