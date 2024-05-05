using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Attributes;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Purchase
{
    [CommandScope(CommandScope.Purchase)]
    [CommandDisplayName("Удалить последнюю трату")]
    [CommandCallback("/deletelastpurchase")]
    public class DeleteLastPurchaseCommand(ICustomLogger logger, IPurchaseService purchaseService) : ICommand
    {
        private readonly ICustomLogger _logger = logger;
        private readonly IPurchaseService _purchaseService = purchaseService;

        public async Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            await _logger.Info($"Received DeleteLastPurchaseAsync. UserId: {message.UserId}");
            var responseText = await _purchaseService.DeleteLastPurchaseAsync(message.UserId);
            return new CommandResult(responseText);
        }
    }
}
