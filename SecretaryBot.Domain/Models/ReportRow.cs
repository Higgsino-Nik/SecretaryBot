namespace SecretaryBot.Domain.Models
{
    public class ReportRow
    {
        public string Name { get; set; }
        public int Amount { get; set; }

        public override string ToString() => $"{Name}: {Amount}";
    }
}
