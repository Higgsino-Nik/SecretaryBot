using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Exceptions;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Purchase
{
    public class AddPurchaseCommand(ICustomLogger logger, ICacheService cacheService, ICategoryService categoryService, IPurchaseService purchaseService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly ICacheService _cacheService = cacheService;
        private readonly ICategoryService _categoryService = categoryService;
        private readonly IPurchaseService _purchaseService = purchaseService;

        public CommandScope Scope => CommandScope.Purchase;
        public string DisplayName => "Зафиксировать трату";
        public string CallBack => "/addpurchase";

        public Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            var commandChain = message.Text.Split('\\');
            return commandChain.Length switch
            {
                1 => DisplayCategories(message.UserId),
                2 => RequestAmountInput(message, int.Parse(commandChain[1])),
                3 => AddPurchase(message.UserId, int.Parse(commandChain[1]), int.Parse(commandChain[2])),
                _ => throw new BadCommandRequestException("Неправильное использование команды")
            };
        }

        private async Task<CommandResult> DisplayCategories(long userId)
        {
            await _logger.Info($"Received AddPurchaseAsync DisplayCategories. UserId: {userId}");
            var categories = await _categoryService.GetCategoriesListAsync(userId);
            var buttons = categories.Select(x => new KeyboardButton { Text = x.Name, CallBackMessage = CallBack + "\\" + x.Id });
            return new CommandResult("Выберите категорию", buttons);
        }

        private async Task<CommandResult> RequestAmountInput(TelegramMessage message, int categoryId)
        {
            await _logger.Info($"Received AddPurchaseAsync RequestAmountInput. UserId: {message.UserId}");
            _cacheService.SetLastCommand(message.UserId, CallBack + "\\" + categoryId);
            return new CommandResult("Введите ЦЕЛОЕ ЧИСЛО - сумма совершенной траты");
        }

        private async Task<CommandResult> AddPurchase(long userId, int categoryId, int amount)
        {
            var category = await _categoryService.GetCategory(categoryId);
            if (category.UserTelegramId != userId)
                return new CommandResult("Возникла ошибка. Выбранная категория не найдена либо не принадлежит вам");

            var responseMessage = await _purchaseService.AddPurchaseAsync(categoryId, amount);
            return new CommandResult(responseMessage);
        }
    }
}
