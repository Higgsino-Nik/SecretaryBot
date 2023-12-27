using SecretaryBot.Domain.Models;

namespace SecretaryBot.Domain.Abstractions.Repositories
{
    public interface IPurchaseRepository
    {
        Task AddPurchaseAsync(Purchase Purchase);
        Task DeleteLastPurchaseAsync(long userId);
        Task<List<ReportRow>> PurchaseReportAsync(long userId, DateTime startDate, DateTime endDate);
        Task<List<ReportRow>> CategoryReportAsync(int categoryId, DateTime startDate, DateTime endDate);
    }
}
