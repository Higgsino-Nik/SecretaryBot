namespace SecretaryBot.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandDisplayNameAttribute(string displayName) : Attribute
    {
        private readonly string _displayName = displayName;

        public string GetDisplayName() => _displayName;
    }
}
