namespace SecretaryBot.Domain.Abstractions.Services
{
    public interface IUserService
    {
        Task AddUserAsync(long userId, long chatId);
        Task<bool> HasAccessAsync(long userId);
        Task ChangeAccessAsync(long userId);
    }
}
