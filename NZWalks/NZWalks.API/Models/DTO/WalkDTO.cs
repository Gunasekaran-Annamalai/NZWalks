using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO
{
    public class WalkDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKM { get; set; }
        public string? WalkImageURL { get; set; }

        //public Guid Difficultyid { get; set; }
        //public Guid Regionid { get; set; }

        // Here, we are directly using RegionDTO and DifficultyDTO so there is no need for DifficultyID and RegionID

        public RegionDTO Region { get; set; }
        public DifficultyDTO Difficulty { get; set; }
    }
}
