namespace SecretaryBot.Domain.Models
{
    public record CommandResult
    {
        public string ResponseText { get; set; }
        public List<KeyboardButton> Buttons { get; set; }

        public CommandResult(string text)
        {
            ResponseText = string.IsNullOrEmpty(text) ? "Not found" : text;
            Buttons = [];
        }

        public CommandResult(string text, IEnumerable<KeyboardButton> buttons)
        {
            ResponseText = string.IsNullOrEmpty(text) ? "Not found" : text;
            Buttons = buttons.ToList();
        }
    }
}
