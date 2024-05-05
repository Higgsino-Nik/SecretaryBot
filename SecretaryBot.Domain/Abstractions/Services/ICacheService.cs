namespace SecretaryBot.Domain.Abstractions.Services
{
    public interface ICacheService
    {
        void SetLastCommand(long userId, string command);
        string? GetLastCommand(long userId);
        string? PopLastCommand(long userId);
    }
}
