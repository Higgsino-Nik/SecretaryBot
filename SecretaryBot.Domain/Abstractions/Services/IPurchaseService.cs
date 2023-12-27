namespace SecretaryBot.Domain.Abstractions.Services
{
    public interface IPurchaseService
    {
        Task<string> AddPurchaseAsync(int categoryId, int amount);
        Task<string> DeleteLastPurchaseAsync(long userId);
    }
}
