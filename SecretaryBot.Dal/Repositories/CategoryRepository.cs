using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SecretaryBot.Dal.Models;
using SecretaryBot.Domain.Abstractions.Repositories;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Dal.Repositories
{
    public class CategoryRepository(DbContextOptions<CategoryRepository> options, IMapper mapper) : DbContext(options), ICategoryRepository
    {
        private readonly IMapper _mapper = mapper;
        private DbSet<DalCategory> Categories { get; set; }

        public async Task AddCategoryAsync(Category category)
        {
            var currentCategory = await Categories.FirstOrDefaultAsync(x => x.UserTelegramId == category.UserTelegramId && x.Name == category.Name);
            if (currentCategory is null)
            {
                var dalCategory = _mapper.Map<DalCategory>(category);
                await Categories.AddAsync(dalCategory);
            }
            else
            {
                currentCategory.IsActive = true;
            }
            
            await SaveChangesAsync();
        }

        public async Task<List<Category>> GetActiveCategoriesAsync(long userId) =>
            await Categories.Where(x => x.UserTelegramId == userId && x.IsActive).ProjectTo<Category>(_mapper.ConfigurationProvider).ToListAsync();

        public async Task<Category?> GetCategoryAsync(int categoryId) =>
            await Categories.Where(x => x.Id == categoryId).ProjectTo<Category>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

        public async Task DeactivateCategoryAsync(int categoryId)
        {
            var category = await Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
            if (category != null)
            {
                category.IsActive = false;
                await SaveChangesAsync();
            }
        }
    }
}
