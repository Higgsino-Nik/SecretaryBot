using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Attributes;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Report
{
    [CommandScope(CommandScope.Report)]
    [CommandDisplayName("Отчет по тратам, текущий месяц")]
    [CommandCallback("/currentmonthpurchasereport")]
    public class CurrentMonthPurchaseReportCommand(ICustomLogger logger, IReportService reportService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly IReportService _reportService = reportService;

        public async Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            await _logger.Info($"Received CurrentMonthPurchaseReportAsync. UserId: {message.UserId}");
            var start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var end = start.AddMonths(1);
            var report = await _reportService.GetPurchaseReportAsync(message.UserId, start, end);
            return new CommandResult(report);
        }
    }
}
