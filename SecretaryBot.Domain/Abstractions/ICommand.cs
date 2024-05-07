using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Domain.Abstractions
{
    public interface ICommand
    {
        abstract CommandScope Scope { get; }
        abstract string DisplayName { get; }
        abstract string CallBack { get; }

        Task<CommandResult> InvokeAsync(TelegramMessage message);
    }
}
