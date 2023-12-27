namespace SecretaryBot.Domain.Abstractions.Services
{
    public interface IReportService
    {
        Task<string> GetPurchaseReportAsync(long userId, DateTime start, DateTime end);
        Task<string> GetCategoryReportAsync(int categoryId, DateTime start, DateTime end);
    }
}
