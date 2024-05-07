using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Commands.Report
{
    public class ReportScopeCommand(CommandFactory commandFactory) : ICommand
    {
        private readonly CommandFactory _commandFactory = commandFactory;

        public CommandScope Scope => CommandScope.None;
        public string DisplayName => "Отчеты";
        public string CallBack => "/report";

        public async Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            var reportCommands = _commandFactory.GetCommands(CommandScope.Report);
            var buttons = await Task.Run(() => reportCommands.Select(command => new KeyboardButton { Text = command.DisplayName, CallBackMessage = command.CallBack }));
            return new CommandResult("Отчеты", buttons);
        }
    }
}
