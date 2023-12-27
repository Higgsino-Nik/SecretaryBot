namespace SecretaryBot.Domain.Models
{
    public record TelegramMessage(long UserId, long ChatId, string Text);
}
