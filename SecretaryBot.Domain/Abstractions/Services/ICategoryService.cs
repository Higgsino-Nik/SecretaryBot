using SecretaryBot.Domain.Models;

namespace SecretaryBot.Domain.Abstractions.Services
{
    public interface ICategoryService
    {
        Task<string> AddCategoryAsync(long userId, string categoryName);
        Task<string> AddDefaultCategoriesAsync(long userId);
        Task<string> GetCategoriesAsync(long userId);
        Task<Category> GetCategory(int id);
        Task<List<Category>> GetCategoriesListAsync(long userId);
        Task<string> DeleteCategoryAsync(long userId, int categoryId);
    }
}
