using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/regions -> "regions" it is taking from class name which is "RegionsController"
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            // Get Data from Database - Domain Models
            var regionsDomain = _dbContext.Regions.ToList();

            // Map Domain Models to DTOs
            var regionsDTO = new List<RegionDTO>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDTO.Add(new RegionDTO()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageURL = regionDomain.RegionImageURL
                });
            }

            // Return DTOs back to the client (we should never return Doamin model)
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            // this method, which is select using "Find()" will only match Primary key,
            // it won't support any other field
            //var region = _dbContext.Regions.Find(id);

            // using the below method we can use any field to match and select: Example - 'Code' field.
            // Get Region Domain Model
            var regionDomain = _dbContext.Regions.FirstOrDefault(r => r.Id == id);

            if (regionDomain == null)
            {
                return NotFound("Item can't be found");
            }

            // Map / Convert Region Domain model to Region DTO
            var regionsDTO = new RegionDTO
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageURL = regionDomain.RegionImageURL
            };
            
            // Return DTO
            return Ok(regionDomain);
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            if (addRegionRequestDTO is null)
            {
                return BadRequest("Not inserted");
            }
            // Map or Convert DTO to Domain model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageURL = addRegionRequestDTO.RegionImageURL
            };

            // Use Domain model to create a new Region
            _dbContext.Regions.Add(regionDomainModel);
            _dbContext.SaveChanges();

            // Map Domain back to DTO (it is not necessary if we don't send any data back)
            var regionDTO = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageURL = regionDomainModel.RegionImageURL
            };

            /* In the following step, after adding new item we are getting it back from databse using Id
            // to show it to the client (we can just return any response we want: this is just to learn)

            // Creates a CreatedAtActionResult object that produces a Status201Created response.

            * CreatedAtAction(String, Object, Object)
             * Parameters
             * actionName -> String
             * The name of the action to use for generating the URL.
             * 
             * routeValues -> Object
             * The route data to use for generating the URL.
             * value -> Object
             * The content value to format in the entity body.*/

            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            // check if region exists
            var regionDomainModel = _dbContext.Regions.FirstOrDefault(r => r.Id == id);
            
            if (regionDomainModel == null) return NotFound();
            
            // Map DTO to Domain
            regionDomainModel.Code = updateRegionRequestDTO.Code;
            regionDomainModel.Name = updateRegionRequestDTO.Name;
            regionDomainModel.RegionImageURL = updateRegionRequestDTO.RegionImageURL;

            _dbContext.SaveChanges();

            // Convert Domain to DTO
            var regionDTO = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageURL = regionDomainModel.RegionImageURL
            };

            return Ok(regionDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel = _dbContext.Regions.FirstOrDefault(r => r.Id == id);
            if(regionDomainModel == null) return NotFound();

            // Delete region
            _dbContext.Remove(regionDomainModel);
            _dbContext.SaveChanges();

            // if we want to return the deleted item just follow the steps from update from 'Convert Domain to DTO and return DTO'
            return Ok();
        }
    }
}
