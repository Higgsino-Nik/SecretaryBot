using Microsoft.Extensions.DependencyInjection;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Attributes;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Extensions;
using SecretaryBot.Domain.Models;
using System.Reflection;

namespace SecretaryBot.Bll.Commands.Category
{
    [CommandScope(CommandScope.None)]
    public class CategoryScopeCommand : ICommand
    {
        private readonly IEnumerable<ICommand> _categoryCommands;

        public CategoryScopeCommand(IServiceProvider serviceProvider)
        {
            var commands = new List<ICommand>();
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAssignableTo(typeof(ICommand)) && x.Namespace.Equals(GetType().Namespace) && x != GetType());
            foreach (var type in types)
            {
                commands.Add(serviceProvider.GetRequiredService(type) as ICommand);
            }
            _categoryCommands = commands.ValidateByScope(CommandScope.Category);
        }

        public async Task<CommandResult> InvokeAsync(TelegramMessage message)
        {
            var buttons = await Task.Run(() => _categoryCommands.Select(command => new KeyboardButton { Text = command.GetDisplayName(), CallBackMessage = command.GetCallback() }));
            return new CommandResult("Категории", buttons);
        }
    }
}
