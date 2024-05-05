using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Attributes;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Exceptions;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Category
{
    [CommandScope(CommandScope.Category)]
    [CommandDisplayName("Добавить категорию")]
    [CommandCallback("/addcategory")]
    public class AddCategoryCommand(ICustomLogger logger, ICategoryService categoryService, ICacheService cacheService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly ICategoryService _categoryService = categoryService;
        private readonly ICacheService _cacheService = cacheService;

        public Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            var commandChain = message.Text.Split('\\');
            return commandChain.Length switch
            {
                1 => RequestNameInput(message.UserId),
                2 => AddCategory(message.UserId, commandChain[1]),
                _ => throw new BadCommandRequestException("Неправильное использование команды")
            };
        }

        private async Task<CommandResult> RequestNameInput(long userId)
        {
            await _logger.Info($"Received AddCategory. UserId: {userId}");
            _cacheService.SetLastCommand(userId, "/addcategory");
            return new CommandResult("Введите название новой категории");
        }

        private async Task<CommandResult> AddCategory(long userId, string name)
        {
            await _logger.Info($"Received parameter for command addcategory. User id: {userId}");
            var responseMessage = await _categoryService.AddCategoryAsync(userId, name);
            return new CommandResult(responseMessage);
        }
    }
}
