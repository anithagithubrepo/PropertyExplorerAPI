namespace PropertyExplorerAPI.Models
{
    public class Transportation
    {
        public string Type { get; set; } = default!;
        public string? Line { get; set; }
        public string? Station { get; set; }
        public string Distance { get; set; } = default!;
    }
}
