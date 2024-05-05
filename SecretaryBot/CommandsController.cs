using Microsoft.Extensions.Configuration;
using SecretaryBot.Bll.Commands;
using SecretaryBot.Domain;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Models;

namespace SecretaryBot
{
    public class CommandsController(ICustomLogger logger, ICacheService cacheService, 
        IUserService userService, IConfiguration configuration, CommandFactory commandFactory)
    {
        private readonly ICustomLogger _logger = logger;
        private readonly CommandFactory _commandFactory = commandFactory;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IUserService _userService = userService;

        private readonly long ADMIN_ID = long.Parse(configuration["AdminId"]);

        public async Task<CommandResult> ResponseCommandAsync(TelegramMessage request)
        {
            await _logger.Info($"Received start. UserId: {request.UserId}");

            var hasAccess = await _userService.HasAccessAsync(request.UserId);
            if (request.Text != "/start" && request.Text != "/help" && !hasAccess)
                return new CommandResult(Constants.PermissionDeniedResponse + request.UserId);

            request.Text = request.Text.StartsWith('/')
                ? request.Text
                : _cacheService.GetLastCommand(request.UserId) + "\\" + request.Text;

            var command = _commandFactory.CreateCommand(request.Text);
            var commandResult = await command.InvokeAsync(request);
            return commandResult;
        }
    }
}
