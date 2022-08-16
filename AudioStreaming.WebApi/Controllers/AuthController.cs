using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using AudioStreaming.Security.Models;
using AudioStreaming.Security.Services.Abstractions;

namespace AudioStreaming.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;

        public AuthController(ITokenService tokenService, IAuthService authService)
        {
            _tokenService = tokenService;
            _authService = authService;
        }


        [HttpPost("[action]")]
        public async Task<IApiResult> Registration([FromBody]UserRegistrationModel payload)
        {
            var result = await _authService.CreateNewUserAsync(payload);
            
            return result.UserId != null ? ApiResult.CreateSuccessfulResult() : ApiResult.CreateFailedResult(result.Errors);
        }

        [HttpPost("[action]")]
        public async Task<IApiResult<AuthenticatedResponse>> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null)
            {
                return ApiResult<AuthenticatedResponse>.CreateFailedResult("Invalid client request.");
            }

            var userId = await _authService.ValidateUser(loginModel);

            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<AuthenticatedResponse>.CreateFailedResult("Wrong password or username.");
            }

            var response = new AuthenticatedResponse();

            response.AccessToken = await _tokenService.GenerateAccessToken(userId);
            response.RefreshToken = await _tokenService.UpdateRefreshToken(userId);

            return ApiResult<AuthenticatedResponse>.CreateSuccessfulResult(response);
        }
    }
}
