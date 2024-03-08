using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create([FromBody] AddWalkResuestDTO addWalkResuestDTO)
        {
            // Map DTO to Domain
            var walkDomainModel = _mapper.Map<Walk>(addWalkResuestDTO);

            await _walkRepository.CreateAsync(walkDomainModel);

            // Map Domain to DTO
            return Ok(_mapper.Map<AddWalkResuestDTO>(walkDomainModel));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walksDomainModel = await _walkRepository.GetAllAsync();
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
