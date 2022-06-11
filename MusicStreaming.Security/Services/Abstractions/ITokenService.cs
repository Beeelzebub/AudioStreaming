using System.Security.Claims;

namespace MusicStreaming.Security.Services.Abstractions
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(string userId);

        Task<string> UpdateRefreshToken(string userId);

        (bool IsValid, ClaimsPrincipal? Principal) ValidateExpiredToken(string accessToken);

        Task<bool> ValidateRefreshToken(string userId, string refreshToken);

        Task RevokeRefreshToken(string userId);
    }
}
