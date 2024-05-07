using SecretaryBot.Domain;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands
{
    public class StartCommand(ICustomLogger logger, IUserService userService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly IUserService _userService = userService;

        public CommandScope Scope => CommandScope.None;
        public string DisplayName => "Старт";
        public string CallBack => "/start";

        public async Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            await _logger.Info($"Received start. UserId: {message.UserId}");
            await _userService.AddUserAsync(message.UserId, message.ChatId);
            return new CommandResult(Constants.StartMessageResponse + message.UserId);
        }
    }
}
