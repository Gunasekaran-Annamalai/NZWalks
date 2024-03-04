using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/regions -> "regions" it is taking from class name which is "RegionsController"
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get Data from Database - Domain Models
            var regionsDomain = await _regionRepository.GetAllAsync();

            // Map Domain Models to DTOs
            // This is to map Domain Model to DTO
            // Return DTOs back to the client (we should never return Doamin model)
            return Ok(_mapper.Map<List<RegionDTO>>(regionsDomain)); // -> regionsDomain: it is our source & List<RegionDTO>: is out type
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // this method, which is select using "Find()" will only match Primary key,
            // it won't support any other field
            //var region = _dbContext.Regions.Find(id);

            // using the below method we can use any field to match and select: Example - 'Code' field.
            // Get Region Domain Model
            var regionDomain = await _regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound("Item can't be found");
            }

            // Map / Convert Region Domain model to Region DTO
            // Return DTO
            return Ok(_mapper.Map<RegionDTO>(regionDomain));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            if (addRegionRequestDTO is null)
            {
                return BadRequest("Not inserted");
            }
            // Map or Convert DTO to Domain model
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDTO);

            // Use Domain model to create a new Region
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

            // Map Domain back to DTO (it is not necessary if we don't send any data back)
            var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);

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
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            // Map DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDTO);

            // we are converting DTO - from user; to Domain - to database; and passing it to our UpdateAsync
            regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null) return NotFound();

            // Convert Domain to DTO
            // We are returning the DTO but if you don't want to return any data below step won't be necessary

            return Ok(_mapper.Map<RegionDTO>(regionDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            if (regionDomainModel == null) return NotFound();

            return Ok(_mapper.Map<RegionDTO>(regionDomainModel));
        }
    }
}
