using SecretaryBot.Domain.Enums;

namespace SecretaryBot.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandScopeAttribute(CommandScope scope) : Attribute
    {
        private readonly CommandScope _scope = scope;

        public CommandScope GetScope() => _scope;
        public bool InScope(CommandScope scope) => _scope == scope;
    }
}
