using SecretaryBot.Domain;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands
{
    public class HelpCommand(ICustomLogger logger) : ICommand
    {
        private readonly ICustomLogger _logger = logger;

        public CommandScope Scope => CommandScope.None;
        public string DisplayName => "Справочная информация";
        public string CallBack => "/help";

        public async Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            await _logger.Info($"Received help. UserId: {message.UserId}");
            return new CommandResult(Constants.CommandsDocumentation);
        }
    }
}
