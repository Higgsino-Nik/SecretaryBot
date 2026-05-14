using SecretaryBot.Domain;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Exceptions;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Report
{
    public class LastMonthCategoryReportCommand(ICustomLogger logger, IReportService reportService, ICategoryService categoryService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly IReportService _reportService = reportService;
        private readonly ICategoryService _categoryService = categoryService;

        public CommandScope Scope => CommandScope.Report;
        public string DisplayName => "Отчет по категории, прошлый месяц";
        public string CallBack => "/lastmonthcategoryreport";

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
                    return DisplayReport(message, categoryId);
                default:
                    throw new BadCommandRequestException("Неправильное использование команды");
            }
        }

        private async Task<CommandResult> DisplayCategories(long userId)
        {
            await _logger.InfoAsync($"Received LastMonthCategoryReportAsync. UserId: {userId}");
            var categories = await _categoryService.GetCategoriesListAsync(userId);
            var buttons = categories.Select(x => new KeyboardButton { Text = x.Name, CallBackMessage = CallBack + Constants.CommandInputSeparator + x.Id });
            return new CommandResult("Выберите категорию", buttons);
        }

        private async Task<CommandResult> DisplayReport(TelegramMessage message, int categoryId)
        {
            var currentMonthFirstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var previousMonthFirstDay = currentMonthFirstDay.AddMonths(-1);
            var category = await _categoryService.GetCategory(categoryId);
            if (category is null || category.UserTelegramId != message.UserId)
                return new CommandResult("Возникла ошибка. Выбранная категория не найдена либо не принадлежит вам");

            await _logger.InfoAsync($"Received parameter for command addcategory. User id: {message.UserId}");
            var responseMessage = await _reportService.GetCategoryReportAsync(categoryId, previousMonthFirstDay, currentMonthFirstDay);
            return new CommandResult(responseMessage);
        }

        private static bool TryParseCategoryId(string value, out int categoryId) =>
            int.TryParse(value, out categoryId) && categoryId > 0;
    }
}
