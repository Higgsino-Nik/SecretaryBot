using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SecretaryBot.Dal.Models;
using SecretaryBot.Domain.Abstractions.Repositories;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Dal.Repositories
{
    public class CategoryRepository(IMapper mapper, PostgresContext context) : ICategoryRepository
    {
        private readonly IMapper _mapper = mapper;
        private readonly PostgresContext _context = context;

        public async Task AddCategoryAsync(Category category)
        {
            var currentCategory = await _context.Categories.FirstOrDefaultAsync(x => x.UserTelegramId == category.UserTelegramId && x.Name == category.Name);
            if (currentCategory is null)
            {
                var dalCategory = _mapper.Map<DalCategory>(category);
                await _context.Categories.AddAsync(dalCategory);
            }
            else
            {
                currentCategory.IsActive = true;
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetActiveCategoriesAsync(long userId) =>
            await _context.Categories.Where(x => x.UserTelegramId == userId && x.IsActive).ProjectTo<Category>(_mapper.ConfigurationProvider).ToListAsync();

        public async Task<Category?> GetCategoryAsync(int categoryId) =>
            await _context.Categories.Where(x => x.Id == categoryId).ProjectTo<Category>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

        public async Task DeactivateCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
            if (category != null)
            {
                category.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
