using Microsoft.Extensions.Configuration;
using SecretaryBot.Bll.Commands;
using SecretaryBot.Domain;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Exceptions;
using SecretaryBot.Domain.Models;
using SecretaryBot.Domain.Texts;

namespace SecretaryBot
{
    public class CommandsController(ICustomLogger logger, ICacheService cacheService, 
        IUserService userService, CommandFactory commandFactory)
    {
        private readonly ICustomLogger _logger = logger;
        private readonly CommandFactory _commandFactory = commandFactory;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IUserService _userService = userService;

        public async Task<CommandResult> ResponseCommandAsync(TelegramMessage request)
        {
            await _logger.InfoAsync($"Received start. UserId: {request.UserId}");

            var hasAccess = await _userService.HasAccessAsync(request.UserId);
            if (request.Text != "/start" && request.Text != "/help" && !hasAccess)
                return new CommandResult(BotMessages.PermissionDeniedMessage + request.UserId);

            if (request.Text.StartsWith('/'))
            {
                _cacheService.ClearLastCommand(request.UserId);
            }
            else
            {
                var lastCommand = _cacheService.GetLastCommand(request.UserId);
                if (string.IsNullOrEmpty(lastCommand))
                    throw new BadCommandRequestException("Команда не найдена");

                request.Text = lastCommand + Constants.CommandInputSeparator + request.Text;
            }

            var command = _commandFactory.CreateCommand(request.Text);
            var commandResult = await command.InvokeAsync(request);
            return commandResult;
        }
    }
}
