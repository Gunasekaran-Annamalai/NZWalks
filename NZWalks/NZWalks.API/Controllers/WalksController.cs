using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository) {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkResuestDTO addWalkResuestDTO)
        {
            // Map DTO to Domain
            var walkDomainModel = _mapper.Map<Walk>(addWalkResuestDTO);

            await _walkRepository.CreateAsync(walkDomainModel);

            // Map Domain to DTO
            return Ok(_mapper.Map<AddWalkResuestDTO>(walkDomainModel));
        }

        [HttpGet]
        // Get: /api/walks?filterOn=Field_Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        // filterOn -> name of the field/property that we want to make search/filter on Eg: [filterOn=Name, filterQuery=Alex]
        // filterQuery -> the word/sentence that we need to search
        // sortBy -> the column that we want to be sorted
        // isAscending -> if true ascending else decending

        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn, 
            [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, 
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000)
        {
            // isAscending ?? true -> saying that if isAscending is null then consider it as true
            var walksDomainModel = await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
            // Map Domain to DTO
            return Ok(_mapper.Map<List<WalkDTO>>(walksDomainModel));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.GetByIdAsync(id);
            if (walkDomainModel is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<WalkDTO>(walkDomainModel));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestDTO)
        {
            // Map DTO to Domain
            var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDTO);

            walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDTO>(walkDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var walkDomainModel = await _walkRepository.DeleteAsync(id);

            if (walkDomainModel is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDTO>(walkDomainModel));
        }
    }
}
