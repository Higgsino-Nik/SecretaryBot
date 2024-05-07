using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Category
{
    public class AddDefaultCategoriesCommand(ICustomLogger logger, ICategoryService categoryService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly ICategoryService _categoryService = categoryService;

        public CommandScope Scope => CommandScope.Category;
        public string DisplayName => "Добавить категории по умолчанию";
        public string CallBack => "/adddefaultcategories";

        public async Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            await _logger.Info($"Received AddDefaultCategoriesAsync. UserId: {message.UserId}");
            var result = await _categoryService.AddDefaultCategoriesAsync(message.UserId);
            var responseText = result is null
                ? "Добавление категорий по умолчанию возможно только если на данный момент нет никаких категорий"
                : "Были добавлены дефолтные категории:\n\n" + result;
            return new CommandResult(responseText);
        }
    }
}
