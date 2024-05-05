using SecretaryBot.Domain;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Attributes;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands
{
    [CommandScope(CommandScope.None)]
    public class StartCommand(ICustomLogger logger, IUserService userService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly IUserService _userService = userService;

        public async Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            await _logger.Info($"Received start. UserId: {message.UserId}");
            await _userService.AddUserAsync(message.UserId, message.ChatId);
            return new CommandResult(Constants.StartMessageResponse + message.UserId);
        }
    }
}
