using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Domain.Abstractions
{
    public interface ICommand
    {
        CommandScope Scope { get; }
        string DisplayName { get; }
        string CallBack { get; }

        Task<CommandResult> InvokeAsync(TelegramMessage message);
    }
}
