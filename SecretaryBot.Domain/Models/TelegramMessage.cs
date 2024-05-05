namespace SecretaryBot.Domain.Models
{
    public record TelegramMessage
    {
        public long UserId { get; init; }
        public long ChatId { get; init; }
        public string Text { get; set; }
        public TelegramMessage(long userId, long chatId, string text)
        {
            UserId = userId;
            ChatId = chatId;
            Text = text;
        }
    };
}
