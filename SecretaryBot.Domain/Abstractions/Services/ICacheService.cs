namespace SecretaryBot.Domain.Abstractions.Services
{
    public interface ICacheService
    {
        void AddLastCommand(long userId, string command);
        string? GetLastCommand(long userId);
    }
}
