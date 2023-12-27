using System.ComponentModel.DataAnnotations.Schema;

namespace SecretaryBot.Dal.Models
{
    [Table("User")]
    public class DalUser
    {
        public int Id { get; set; }
        public long TelegramId { get; set; }
        public long ChatId { get; set; }
        public bool HasAccess { get; set; }
    }
}
