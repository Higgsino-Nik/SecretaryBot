using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Attributes;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Exceptions;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Report
{
    [CommandScope(CommandScope.Report)]
    [CommandDisplayName("Отчет по категории, прошлый месяц")]
    [CommandCallback("/lastmonthcategoryreport")]
    public class LastMonthCategoryReportCommand(ICustomLogger logger, IReportService reportService, ICategoryService categoryService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly IReportService _reportService = reportService;
        private readonly ICategoryService _categoryService = categoryService;

        public Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            var commandChain = message.Text.Split('\\');
            return commandChain.Length switch
            {
                1 => DisplayCategories(message.UserId),
                2 => DisplayReport(message, int.Parse(commandChain[1])),
                _ => throw new BadCommandRequestException("Неправильное использование команды")
            };
        }

        private async Task<CommandResult> DisplayCategories(long userId)
        {
            await _logger.Info($"Received LastMonthCategoryReportAsync. UserId: {userId}");
            var categories = await _categoryService.GetCategoriesListAsync(userId);
            var buttons = categories.Select(x => new KeyboardButton { Text = x.Name, CallBackMessage = "/lastmonthcategoryreport\\" + x.Id });
            return new CommandResult("Выберите категорию", buttons);
        }

        private async Task<CommandResult> DisplayReport(TelegramMessage message, int categoryId)
        {
            var currentMonthFirstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var previousMonthFirstDay = currentMonthFirstDay.AddMonths(-1);

            await _logger.Info($"Received parameter for command addcategory. User id: {message.UserId}");
            var responseMessage = await _reportService.GetCategoryReportAsync(categoryId, previousMonthFirstDay, currentMonthFirstDay);
            return new CommandResult(responseMessage);
        }
    }
}
