using AudioStreaming.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using AudioStreaming.Security.Models;
using AudioStreaming.Security.Services.Abstractions;

namespace AudioStreaming.Security.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(string? UserId, ICollection<string>? Errors)> CreateNewUserAsync(UserRegistrationModel registrationModel)
        {
            var user = new User { UserName = registrationModel.UserName };

            var result = await _userManager.CreateAsync(user, registrationModel.Password);

            if (!result.Succeeded)
            {
                var errors = new List<string>();

                foreach (var error in result.Errors)
                {
                    errors.Add($"{error.Code}: {error.Description}");
                }

                return (null, errors);
            }

            return (user.Id, null);
        }

        public async Task<string?> ValidateUser(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            
            return user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password) ? user.Id : null;
        }
    }
}
