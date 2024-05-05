namespace SecretaryBot.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandCallbackAttribute(string callback) : Attribute
    {
        private readonly string _callback = callback;

        public string GetCallBack() => _callback;
    }
}
