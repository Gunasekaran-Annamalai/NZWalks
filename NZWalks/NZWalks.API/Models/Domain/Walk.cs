namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        // The below has null warning: it is fine, we will not give '?' because the below field should not allow null
        public string Description { get; set; }
        public double LengthInKM { get; set; }
        // Putting '?' is to define that the following field may allow null value
        public string? WalkImageURL { get; set; }
        // The below Ids are foreign keys
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

        // Navigation properties
        // Wrinting the properties like this will tell EF to create a relationship between given tables with this table
        // This defines a one-to-one relationship between [Walk & Difficulty] and [Walk & Region]

        // The navigatio property is optional, it is useful when you want to send these data to the client
        // Or it is useful if you need the data for any other process
        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; }
    }
}
