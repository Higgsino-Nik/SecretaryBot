using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Attributes;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Exceptions;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Category
{
    [CommandScope(CommandScope.Category)]
    [CommandDisplayName("Удалить категорию")]
    [CommandCallback("/deletecategory")]
    public class DeleteCategoryCommand(ICustomLogger logger, ICategoryService categoryService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly ICategoryService _categoryService = categoryService;

        public Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            var commandChain = message.Text.Split('\\');
            return commandChain.Length switch
            {
                1 => DisplayCategories(message.UserId),
                2 => DeleteCategory(message, int.Parse(commandChain[1])),
                _ => throw new BadCommandRequestException("Неправильное использование команды")
            };
        }

        private async Task<CommandResult> DisplayCategories(long userId)
        {
            await _logger.Info($"Received DeleteCategoryAsync. UserId: {userId}");
            var categories = await _categoryService.GetCategoriesListAsync(userId);
            var buttons = categories.Select(x => new KeyboardButton { Text = x.Name, CallBackMessage = "/deletecategory\\" + x.Id });
            return new CommandResult("Выберите категорию, которую хотите удалить", buttons);
        }

        private async Task<CommandResult> DeleteCategory(TelegramMessage message, int categoryId)
        {
            await _logger.Info($"Received parameter for command deletecategory. User id: {message.UserId}");
            var responseMessage = await _categoryService.DeleteCategoryAsync(message.UserId, categoryId);
            return new CommandResult(responseMessage);
        }
    }
}
