using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Attributes;
using SecretaryBot.Domain.Enums;

namespace SecretaryBot.Domain.Extensions
{
    public static class CommandExtensions
    {
        public static IEnumerable<ICommand> ValidateByScope(this IEnumerable<ICommand> commands, CommandScope scope) => commands
            .Where(command => command
                    .GetType()
                    .GetCustomAttributes(typeof(CommandScopeAttribute), false)
                    .FirstOrDefault()
                    is CommandScopeAttribute scopeAttribute && scopeAttribute.InScope(scope));

        public static string GetDisplayName(this ICommand command) => command
                    .GetType()
                    .GetCustomAttributes(typeof(CommandDisplayNameAttribute), false)
                    .FirstOrDefault() is not CommandDisplayNameAttribute attribute ? string.Empty : attribute.GetDisplayName();

        public static string GetCallback(this ICommand command) => command
                    .GetType()
                    .GetCustomAttributes(typeof(CommandCallbackAttribute), false)
                    .FirstOrDefault() is not CommandCallbackAttribute attribute ? string.Empty : attribute.GetCallBack();
    }
}
