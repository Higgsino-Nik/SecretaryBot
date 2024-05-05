using SecretaryBot.Domain.Models;

namespace SecretaryBot.Domain.Abstractions
{
    public interface ICommand
    {
        Task<CommandResult> InvokeAsync(TelegramMessage message);
    }
}
