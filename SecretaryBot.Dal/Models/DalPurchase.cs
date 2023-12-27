using System.ComponentModel.DataAnnotations.Schema;

namespace SecretaryBot.Dal.Models
{
    [Table("Purchase")]
    public class DalPurchase
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public DalCategory Category { get; set; }
        public int Amount { get; set; }
        public DateTime PurchaseDateTime { get; set; }
    }
}
