using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO
{
    public class AddWalkResuestDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKM { get; set; }
        public string? WalkImageURL { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

        //public Difficulty Difficulty { get; set; }
        //public Region Region { get; set; }
    }
}
