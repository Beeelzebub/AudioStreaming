using AudioStreaming.Security.Models;

namespace AudioStreaming.Security.Services.Abstractions
{
    public interface IAuthService
    {
        Task<(string? UserId, ICollection<string>? Errors)> CreateNewUserAsync(UserRegistrationModel registrationModel);

        Task<string?> ValidateUser(LoginModel loginModel);
    }
}
