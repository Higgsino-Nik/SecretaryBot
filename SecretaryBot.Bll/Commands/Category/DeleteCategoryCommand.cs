using SecretaryBot.Domain;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Exceptions;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Category
{
    public class DeleteCategoryCommand(ICustomLogger logger, ICategoryService categoryService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly ICategoryService _categoryService = categoryService;

        public CommandScope Scope => CommandScope.Category;
        public string DisplayName => "Удалить категорию";
        public string CallBack => "/deletecategory";

        public Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            var commandChain = message.Text.Split(Constants.CommandInputSeparator);
            switch (commandChain.Length)
            {
                case 1:
                    return DisplayCategories(message.UserId);
                case 2:
                    if (!TryParseCategoryId(commandChain[1], out var categoryId))
                        return Task.FromResult(new CommandResult("Некорректный идентификатор категории"));
                    return DeleteCategory(message, categoryId);
                default:
                    throw new BadCommandRequestException("Неправильное использование команды");
            }
        }

        private async Task<CommandResult> DisplayCategories(long userId)
        {
            await _logger.InfoAsync($"Received DeleteCategoryAsync. UserId: {userId}");
            var categories = await _categoryService.GetCategoriesListAsync(userId);
            var buttons = categories.Select(x => new KeyboardButton { Text = x.Name, CallBackMessage = CallBack + Constants.CommandInputSeparator + x.Id });
            return new CommandResult("Выберите категорию, которую хотите удалить", buttons);
        }

        private async Task<CommandResult> DeleteCategory(TelegramMessage message, int categoryId)
        {
            await _logger.InfoAsync($"Received parameter for command deletecategory. User id: {message.UserId}");
            var responseMessage = await _categoryService.DeleteCategoryAsync(message.UserId, categoryId);
            return new CommandResult(responseMessage);
        }

        private static bool TryParseCategoryId(string value, out int categoryId) =>
            int.TryParse(value, out categoryId) && categoryId > 0;
    }
}
