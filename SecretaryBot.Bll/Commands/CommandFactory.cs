using SecretaryBot.Domain;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Enums;
using SecretaryBot.Domain.Exceptions;

namespace SecretaryBot.Bll.Commands
{
    public class CommandFactory
    {
        private readonly Dictionary<string, ICommand> _commands;

        public CommandFactory(IEnumerable<ICommand> commands)
        {
            _commands = commands.ToDictionary(x => x.CallBack);
        }

        public List<ICommand> GetCommands(CommandScope scope) => _commands.Values.Where(x => x.Scope == scope).ToList();

        public ICommand CreateCommand(string commandText)
        {
            var baseCommand = commandText.Split(Constants.CommandInputSeparator)[0];
            return _commands.TryGetValue(baseCommand, out var command)
                ? command
                : throw new BadCommandRequestException("Команда не найдена");
        }
    }
}
