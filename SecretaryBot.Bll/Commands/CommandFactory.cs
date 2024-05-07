using Microsoft.Extensions.DependencyInjection;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Exceptions;

namespace SecretaryBot.Bll.Commands
{
    public class CommandFactory(IServiceProvider serviceProvider)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public List<ICommand> GetCommands(CommandScope scope) =>
            _serviceProvider.GetServices<ICommand>().Where(x => x.Scope == scope).ToList();

        public ICommand CreateCommand(string commandText)
        {
            var baseCommand = commandText.Split('\\')[0];
            var command = _serviceProvider.GetServices<ICommand>().FirstOrDefault(x => x.CallBack.Equals(baseCommand));
            return command is null
                ? throw new BadCommandRequestException("Команда не найдена")
                : command;
        }
    }
}
