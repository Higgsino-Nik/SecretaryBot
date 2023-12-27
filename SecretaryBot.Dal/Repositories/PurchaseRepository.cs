using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretaryBot.Dal.Models;
using SecretaryBot.Domain.Abstractions.Repositories;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Dal.Repositories
{
    public class PurchaseRepository(DbContextOptions<PurchaseRepository> options, IMapper mapper) : DbContext(options), IPurchaseRepository
    {
        private readonly IMapper _mapper = mapper;
        private DbSet<DalPurchase> Purchases { get; set; }

        public async Task AddPurchaseAsync(Purchase purchase)
        {
            var dalPurchase = _mapper.Map<DalPurchase>(purchase);
            Purchases.Add(dalPurchase);
            await SaveChangesAsync();
        }

        public async Task DeleteLastPurchaseAsync(long userId)
        {
            var lastPurchase = await Purchases
                .Include(x => x.Category)
                .Where(x => x.Category.UserTelegramId == userId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            if (lastPurchase != null)
                Purchases.Remove(lastPurchase);
            await SaveChangesAsync();
        }

        public async Task<List<ReportRow>> PurchaseReportAsync(long userId, DateTime startDate, DateTime endDate)
        {
            return await Purchases.Include(x => x.Category)
                .Where(x => x.Category.UserTelegramId == userId && x.PurchaseDateTime >= startDate && x.PurchaseDateTime <= endDate)
                .GroupBy(x => x.Category.Name)
                .Select(x => new ReportRow { Name = x.Key, Amount = x.Sum(p => p.Amount) })
                .ToListAsync();
        }

        public async Task<List<ReportRow>> CategoryReportAsync(int categoryId, DateTime startDate, DateTime endDate)
        {
            return await Purchases.Include(x => x.Category)
                .Where(x => x.Category.Id == categoryId && x.PurchaseDateTime >= startDate && x.PurchaseDateTime <= endDate)
                .GroupBy(x => x.PurchaseDateTime.Date)
                .Select(x => new ReportRow { Name = x.Key.ToShortDateString(), Amount = x.Sum(p => p.Amount) })
                .ToListAsync();
        }
    }
}
