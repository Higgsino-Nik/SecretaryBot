using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Report
{
    public class CurrentMonthPurchaseReportCommand(ICustomLogger logger, IReportService reportService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly IReportService _reportService = reportService;

        public CommandScope Scope => CommandScope.Report;
        public string DisplayName => "Отчет по тратам, текущий месяц";
        public string CallBack => "/currentmonthpurchasereport";

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
