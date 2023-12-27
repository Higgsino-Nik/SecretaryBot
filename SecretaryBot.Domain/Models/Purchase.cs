namespace SecretaryBot.Domain.Models
{
    public class Purchase
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
        public DateTime PurchaseDateTime { get; set; }
    }
}
