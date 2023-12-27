using SecretaryBot.Domain.Models;

namespace SecretaryBot.Domain.Abstractions.Repositories
{
    public interface ICategoryRepository
    {
        public Task AddCategoryAsync(Category category);
        public Task<List<Category>> GetActiveCategoriesAsync(long userId);
        Task<Category?> GetCategoryAsync(int categoryId);
        public Task DeactivateCategoryAsync(int categoryId);
    }
}
