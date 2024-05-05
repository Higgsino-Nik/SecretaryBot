namespace SecretaryBot.Domain.Exceptions
{
    public class BadCommandRequestException(string message) : Exception(message)
    {
    }
}
