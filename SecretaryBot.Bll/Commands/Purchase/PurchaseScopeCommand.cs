using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Purchase
{
    public class PurchaseScopeCommand(CommandFactory commandFactory) : ICommand
    {
        private readonly CommandFactory _commandFactory = commandFactory;

        public CommandScope Scope => CommandScope.None;
        public string DisplayName => "Покупки";
        public string CallBack => "/purchase";

        public async Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            var purchaseCommands = _commandFactory.GetCommands(CommandScope.Purchase);
            var buttons = await Task.Run(() => purchaseCommands.Select(command => new KeyboardButton { Text = command.DisplayName, CallBackMessage = command.CallBack }));
            return new CommandResult("Траты", buttons);
        }
    }
}
