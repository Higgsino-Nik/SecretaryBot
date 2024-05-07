using SecretaryBot.Domain;
using SecretaryBot.Domain.Abstractions.Repositories;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Services
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        public async Task<string> AddCategoryAsync(long userId, string categoryName)
        {
            var category = new Category
            {
                IsActive = true,
                Name = categoryName,
                UserTelegramId = userId
            };
            await _categoryRepository.AddCategoryAsync(category);
            return "Категория была успешно добавлена! \n\n" + await GetCategoriesAsync(userId);
        }

        public async Task<string?> AddDefaultCategoriesAsync(long userId)
        {
            var currentCategories = await _categoryRepository.GetActiveCategoriesAsync(userId);
            if (currentCategories.Count > 0)
            {
                return null;
            }

            var defaultCategories = Constants.GetDefaultCategories();
            foreach (var category in defaultCategories)
            {
                category.UserTelegramId = userId;
                await _categoryRepository.AddCategoryAsync(category);
            }
            return "Дефолтные категории были успешно добавлены! \n\n" + await GetCategoriesAsync(userId);
        }

        public Task<Category> GetCategory(int id) => _categoryRepository.GetCategoryAsync(id);

        public async Task<string> GetCategoriesAsync(long userId) =>
            "Активные категории:\n" + string.Join("\n",
                (await _categoryRepository.GetActiveCategoriesAsync(userId))
                .Select(x => x.Name));

        public async Task<List<Category>> GetCategoriesListAsync(long userId) =>
            await _categoryRepository.GetActiveCategoriesAsync(userId);

        public async Task<string> DeleteCategoryAsync(long userId, int categoryId)
        {
            var category = await _categoryRepository.GetCategoryAsync(categoryId);
            if (category is null || category.UserTelegramId != userId)
                return "Возникла ошибка при удалении категории: она не найдена либо не принадлежит вам";

            await _categoryRepository.DeactivateCategoryAsync(categoryId);
            return "Категория была успешно удалена! \n\n" + await GetCategoriesAsync(userId);
        }
    }
}
