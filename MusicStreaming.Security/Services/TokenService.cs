using AudioStreaming.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MusicStreaming.Security.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MusicStreaming.Security.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtOptions _jwtOptions;

        private User? _user = null;

        public TokenService(JwtOptions jwtOptions, UserManager<User> userManager)
        {
            _jwtOptions = jwtOptions;
            _userManager = userManager;
        }

        public async Task<string> GenerateAccessToken(string userId)
        {
            var user = await GetUserById(userId);

            if (user == null)
            {
                return string.Empty;
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = await GetUserClaims(user);

            var tokeOptions = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        public async Task<string> UpdateRefreshToken(string userId)
        {
            var user = await GetUserById(userId);

            if (user == null)
            {
                return string.Empty;
            }

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExperation = DateTimeOffset.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            return user.RefreshToken;
        }

        public (bool IsValid, ClaimsPrincipal? Principal) ValidateExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                ValidateLifetime = false 
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken? validatedToken;
            ClaimsPrincipal principal;

            try
            {
                principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            }
            catch (SecurityTokenException)
            {
                return (false, null);
            }

            return (validatedToken != null, principal);
        }

        public async Task<bool> ValidateRefreshToken(string userId, string refreshToken)
        {
            var user = await GetUserById(userId);
            
            return user != null && user.RefreshToken == refreshToken && user.RefreshTokenExperation > DateTimeOffset.UtcNow;
        }

        public async Task RevokeRefreshToken(string userId)
        {
            var user = await GetUserById(userId);

            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExperation = null;
                await _userManager.UpdateAsync(user);
            }
        }

        private async Task<User> GetUserById(string userId) 
            => _user?.Id == userId ? _user : _user = await _userManager.FindByIdAsync(userId);

        private async Task<IList<Claim>> GetUserClaims(User user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
