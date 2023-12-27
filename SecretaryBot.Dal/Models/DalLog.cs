using System.ComponentModel.DataAnnotations.Schema;

namespace SecretaryBot.Dal.Models
{
    [Table("Log")]
    public class DalLog
    {
        public long Id { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public DateTime LogDateTime { get; set; }
    }
}
