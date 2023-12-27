using SecretaryBot.Domain.Abstractions.Repositories;
using SecretaryBot.Domain.Abstractions.Services;

namespace SecretaryBot.Bll.Services
{
    public class ReportService(IPurchaseRepository purchaseRepository) : IReportService
    {
        private readonly IPurchaseRepository _purchaseRepository = purchaseRepository;

        public async Task<string> GetPurchaseReportAsync(long userId, DateTime start, DateTime end)
        {
            if (start > end)
                return "Дата начала для построения отчета должна быть меньше даты конца";

            var report = await _purchaseRepository.PurchaseReportAsync(userId, start, end);
            return string.Join("\n", report.Select(row => row.ToString()));
        }

        public async Task<string> GetCategoryReportAsync(int categoryId, DateTime start, DateTime end)
        {
            if (start > end)
                return "Дата начала для построения отчета должна быть меньше даты конца";

            var report = await _purchaseRepository.CategoryReportAsync(categoryId, start, end);
            return string.Join("\n", report.Select(row => row.ToString()));
        }
    }
}
