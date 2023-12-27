namespace SecretaryBot.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public long UserTelegramId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
