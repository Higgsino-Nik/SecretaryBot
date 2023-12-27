using SecretaryBot.Domain.Models;

namespace SecretaryBot.Domain.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<bool> CheckAccessAsync(long userId);
        Task ChangeAccessAsync(long userId);
        Task GiveAccessAsync(long telegramId);
    }
}
