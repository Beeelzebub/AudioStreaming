using MusicStreaming.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreaming.Security.Services.Abstractions
{
    public interface IAuthService
    {
        Task<(string? UserId, ICollection<string>? Errors)> CreateNewUserAsync(UserRegistrationModel registrationModel);

        Task<string?> ValidateUser(LoginModel loginModel);
    }
}
