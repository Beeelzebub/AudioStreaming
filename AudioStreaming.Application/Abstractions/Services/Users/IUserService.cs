using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStreaming.Application.Abstractions.Services.Users
{
    public interface IUserService
    {
        Task AddRoleToUserAsync(string userId, string role);
    }
}
