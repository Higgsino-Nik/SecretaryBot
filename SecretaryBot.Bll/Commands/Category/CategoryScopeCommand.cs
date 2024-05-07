using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Category
{
    public class CategoryScopeCommand(CommandFactory commandFactory) : ICommand
    {
        private readonly CommandFactory _commandFactory = commandFactory;

        public CommandScope Scope => CommandScope.None;
        public string DisplayName => "Категории";
        public string CallBack => "/category";

        public async Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            var categoryCommands = _commandFactory.GetCommands(CommandScope.Category);
            var buttons = await Task.Run(() => categoryCommands.Select(command => new KeyboardButton { Text = command.DisplayName, CallBackMessage = command.CallBack }));
            return new CommandResult("Категории", buttons);
        }
    }
}
