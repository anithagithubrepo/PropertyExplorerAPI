namespace PropertyExplorerAPI.Models
{
    public class Property
    {
        public string PropertyId { get; set; } = default!;
        public string PropertyName { get; set; } = default!;
        public List<string> Features { get; set; } = new();
        public List<string> Highlights { get; set; } = new();
        public List<Transportation> Transportation { get; set; } = new();
        public List<Space> Spaces { get; set; } = new();
    }

}
