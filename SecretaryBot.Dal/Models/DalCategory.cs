using System.ComponentModel.DataAnnotations.Schema;

namespace SecretaryBot.Dal.Models
{
    [Table("Category")]
    public class DalCategory
    {
        public int Id { get; set; }
        public long UserTelegramId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
