namespace SecretaryBot.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public long TelegramId { get; set; }
        public long ChatId { get; set; }
        public bool HasAccess { get; set; }
    }
}
