using SecretaryBot.Domain;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Attributes;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands
{
    [CommandScope(CommandScope.None)]
    public class HelpCommand(ICustomLogger logger) : ICommand
    {
        private readonly ICustomLogger _logger = logger;

        public async Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            await _logger.Info($"Received help. UserId: {message.UserId}");
            return new CommandResult(Constants.CommandsDocumentation);
        }
    }
}
