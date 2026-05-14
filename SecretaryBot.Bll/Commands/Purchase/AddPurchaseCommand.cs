using SecretaryBot.Domain;
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
            var commandChain = message.Text.Split(Constants.CommandInputSeparator);
            switch (commandChain.Length)
            {
                case 1:
                    return DisplayCategories(message.UserId);
                case 2:
                    return !TryParseIntParameter(commandChain[1], out var categoryId)
                        ? Task.FromResult(new CommandResult("Некорректный идентификатор категории"))
                        : RequestAmountInput(message, categoryId);
                case 3:
                    if (!TryParseIntParameter(commandChain[1], out categoryId))
                        return Task.FromResult(new CommandResult("Некорректный идентификатор категории"));

                    if (!TryParseIntParameter(commandChain[2], out var amount))
                        return Task.FromResult(new CommandResult("Введите положительное целое число - сумма совершенной траты"));

                    return AddPurchase(message.UserId, categoryId, amount);
                default:
                    throw new BadCommandRequestException("Неправильное использование команды");
            }
        }

        private async Task<CommandResult> DisplayCategories(long userId)
        {
            await _logger.InfoAsync($"Received AddPurchaseAsync DisplayCategories. UserId: {userId}");
            var categories = await _categoryService.GetCategoriesListAsync(userId);
            var buttons = categories.Select(x => new KeyboardButton
            {
                Text = x.Name, CallBackMessage = CallBack + Constants.CommandInputSeparator + x.Id
            });
            return new CommandResult("Выберите категорию", buttons);
        }

        private async Task<CommandResult> RequestAmountInput(TelegramMessage message, int categoryId)
        {
            await _logger.InfoAsync($"Received AddPurchaseAsync RequestAmountInput. UserId: {message.UserId}");
            _cacheService.SetLastCommand(message.UserId, CallBack + Constants.CommandInputSeparator + categoryId);
            return new CommandResult("Введите ЦЕЛОЕ ЧИСЛО - сумма совершенной траты");
        }

        private async Task<CommandResult> AddPurchase(long userId, int categoryId, int amount)
        {
            var category = await _categoryService.GetCategory(categoryId);
            if (category is null || category.UserTelegramId != userId)
                return new CommandResult("Возникла ошибка. Выбранная категория не найдена либо не принадлежит вам");

            var responseMessage = await _purchaseService.AddPurchaseAsync(categoryId, amount);
            _cacheService.ClearLastCommand(userId);
            return new CommandResult(responseMessage);
        }

        private static bool TryParseIntParameter(string value, out int result) =>
            int.TryParse(value, out result) && result > 0;
    }
}
