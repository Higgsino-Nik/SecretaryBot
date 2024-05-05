using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Attributes;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Category
{
    [CommandScope(CommandScope.Category)]
    [CommandDisplayName("Список текущих категорий")]
    [CommandCallback("/getcategories")]
    public class GetCategoriesCommand(ICustomLogger logger, ICategoryService categoryService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly ICategoryService _categoryService = categoryService;

        public async Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            await _logger.Info($"Received GetCategoriesAsync. UserId: {message.UserId}");
            var responseText = await _categoryService.GetCategoriesAsync(message.UserId);
            return new CommandResult(responseText);
        }
    }
}
