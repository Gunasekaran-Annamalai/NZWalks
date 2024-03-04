namespace NZWalks.API.Models.Domain
{
    public class Region
    {
        // Guid: It is a random Id which will be automatically generated
        // we don't want to create any object for Guid
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageURL { get; set; }
    }
}
