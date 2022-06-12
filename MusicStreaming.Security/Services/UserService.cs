using AudioStreaming.Application.Abstractions.Services.Users;
using AudioStreaming.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MusicStreaming.Security.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<User> userManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task AddRoleToUserAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, role);
                
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"Add role to user with id {userId} failed: {error.Code} -- {error.Description}.");
                    }
                }
            }
            else
            {
                _logger.LogError($"Add role to user failed: user with id {userId} not found.");
            }
        }
    }
}
