using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // CreateMap<Region, RegionDTO>(); -> this is just for one way mapping
            // if you want to reverse map the above you can use,
            // CreateMap<RegionDTO, Region>() this code or you can use the below code as well

            // Mapping Region - Domain model to RegionDTO - DTO Model
            CreateMap<Region, RegionDTO>().ReverseMap();
            /* Note: ReverMap() is used when we have reverse connection. 
             * Here the connect is just one way
             * but we use ReverseMap() for learning purposes and let us to the concept */
            // Mapping AddRegionRequestDTO - DTO Model to Region - Domain Model
            CreateMap<AddRegionRequestDTO, Region>().ReverseMap();

            CreateMap<UpdateRegionRequestDTO, Region>().ReverseMap();
        }
    }
}
