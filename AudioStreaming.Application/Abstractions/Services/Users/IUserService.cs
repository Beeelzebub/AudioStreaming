namespace AudioStreaming.Application.Abstractions.Services.Users
{
    public interface IUserService
    {
        Task AddRoleToUserAsync(string userId, string role);
    }
}
