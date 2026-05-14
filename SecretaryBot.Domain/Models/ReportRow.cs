namespace SecretaryBot.Domain.Models
{
    public class ReportRow
    {
        public required string Name { get; set; }
        public int Amount { get; set; }

        public override string ToString() => $"{Name}: {Amount}";
    }
}
