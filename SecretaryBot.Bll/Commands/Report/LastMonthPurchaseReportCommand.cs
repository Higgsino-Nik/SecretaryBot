using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Attributes;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Report
{
    [CommandScope(CommandScope.Report)]
    [CommandDisplayName("Отчет по тратам, прошлый месяц")]
    [CommandCallback("/lastmonthpurchasereport")]
    public class LastMonthPurchaseReportCommand(ICustomLogger logger, IReportService reportService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly IReportService _reportService = reportService;

        public async Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            await _logger.Info($"Received LastMonthPurchaseReportAsync. UserId: {message.UserId}");
            var end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var start = end.AddMonths(-1);
            var report = await _reportService.GetPurchaseReportAsync(message.UserId, start, end);
            return new CommandResult(report);
        }
    }
}
