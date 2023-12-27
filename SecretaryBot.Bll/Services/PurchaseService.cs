using SecretaryBot.Domain.Abstractions.Repositories;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Services
{
    public class PurchaseService(IPurchaseRepository purchaseRepository) : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository = purchaseRepository;

        public async Task<string> AddPurchaseAsync(int categoryId, int amount)
        {
            var purchase = new Purchase
            {
                Amount = amount,
                PurchaseDateTime = DateTime.Now,
                CategoryId = categoryId
            };
            await _purchaseRepository.AddPurchaseAsync(purchase);
            return "Трата была успешно зафиксирована";
        }

        public async Task<string> DeleteLastPurchaseAsync(long userId)
        {
            await _purchaseRepository.DeleteLastPurchaseAsync(userId);
            return "Последняя трата была успешно удалена";
        }
    }
}
