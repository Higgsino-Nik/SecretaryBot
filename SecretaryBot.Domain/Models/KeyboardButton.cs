namespace SecretaryBot.Domain.Models
{
    public record KeyboardButton
    {
        public string Text { get; set; }
        public string CallBackMessage { get; set; }
    }
}
