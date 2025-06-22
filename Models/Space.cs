namespace PropertyExplorerAPI.Models
{
    public class Space
    {
        public string SpaceId { get; set; } = default!;
        public string SpaceName { get; set; } = default!;
        public List<RentRoll> RentRoll { get; set; } = new();
    }
}
