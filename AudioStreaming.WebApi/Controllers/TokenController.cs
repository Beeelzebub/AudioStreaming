using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStreaming.Security.Models;
using MusicStreaming.Security.Services.Abstractions;
using System.Security.Claims;

namespace AudioStreaming.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("[action]")]
        public async Task<IApiResult<AuthenticatedResponse>> Refresh([FromBody] TokenApiModel payload)
        {
            if (payload == null || payload.AccessToken == null || payload.RefreshToken == null)
            {
                return ApiResult<AuthenticatedResponse>.CreateFailedResult("Invalid client request.");
            }

            var (isTokenValid, principal) = _tokenService.ValidateExpiredToken(payload.AccessToken);

            if(!isTokenValid)
            {
                return ApiResult<AuthenticatedResponse>.CreateFailedResult("Invalid access token.");
            }

            var response = new AuthenticatedResponse();

            var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return ApiResult<AuthenticatedResponse>.CreateFailedResult("Invalid access token.");
            }

            if (!await _tokenService.ValidateRefreshToken(userId, payload.RefreshToken))
            {
                return ApiResult<AuthenticatedResponse>.CreateFailedResult("Invalid refresh token.");
            }

            response.AccessToken = await _tokenService.GenerateAccessToken(userId);
            response.RefreshToken = await _tokenService.UpdateRefreshToken(userId);

            return ApiResult<AuthenticatedResponse>.CreateSuccessfulResult(response);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IApiResult> Revoke()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return ApiResult.CreateFailedResult("Invalid client request.");
            }

            await _tokenService.RevokeRefreshToken(userId);

            return ApiResult.CreateSuccessfulResult();
        }
    }
}
