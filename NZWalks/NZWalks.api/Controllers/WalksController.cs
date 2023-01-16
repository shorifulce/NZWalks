using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.api.Repositories;

namespace NZWalks.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalksRepository walkRepository;
        private readonly IMapper mapper;

     
        public WalksController(IWalksRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllWalksAync()
        {
            var walks = await walkRepository.GetAllAsync();
            
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);

            return Ok(walksDTO);

            
        }


        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walk = await walkRepository.GetAsync(id);

            if (walk == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);


        }


        [HttpPost]
        public async Task<IActionResult> AddWalkjAsync(Models.DTO.AddWalksRequest addWalksRequest)
        {
            // Request DTO to Domain Model

            var walkDomain = new Models.Domain.Walk()
            {
                Length = addWalksRequest.Length,
                Name = addWalksRequest.Name,
                RegionId = addWalksRequest.RegionId,
                WalkDifficultyID = addWalksRequest.WalkDifficultyID,
                
            };


            // Pass details means Domain class info to Repository

            walkDomain = await walkRepository.AddAsync(walkDomain);

            // Convert Back to DTO (Domain class to DTO for sending it client cz Domain class should not be outside)
            var walkDTO = new Models.DTO.Walk()
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyID = walkDomain.WalkDifficultyID,
            };

            //name of name of function, so name of this method and then we will pass the object value back. 
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);

        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalksRequest updateWalksRequest)
        {
            // Request DTO to Domain Model

            var walkDomain = new Models.Domain.Walk()
            {
                Length = updateWalksRequest.Length,
                Name = updateWalksRequest.Name,
                RegionId = updateWalksRequest.RegionId,
                WalkDifficultyID = updateWalksRequest.WalkDifficultyID,
                
            };


            // Pupdate region using Repository

            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            // if ull then notfound

            if (walkDomain == null)
            {
                return NotFound();
            }

            // Convert Domain to DTO 
            var walknDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyID = walkDomain.WalkDifficultyID,
            };

            // return ok

            return Ok(walknDTO);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalknAsync(Guid id)
        {
            //Get  region from Database

            var walkDomain = await walkRepository.DeleteAsync(id);

            // iff null not found

            if (walkDomain == null)
            {
                return NotFound();
            }

            // convert response back to DTO

            //var walkDTO = new Models.DTO.Walk
            //{
            //    Id = walkDomain.Id,
            //    Name = walkDomain.Name,
            //    Length = walkDomain.Length,
            //    RegionId = walkDomain.RegionId,
            //    WalkDifficultyID = wawalkDomainlk.WalkDifficultyID,


            //}; insted of abouve we can use Automapper

            var walkDTO= mapper.Map<Models.DTO.Walk>(walkDomain);
            // walkDomain model has shifted or move into DTO model cz we expose DTO class

            // return OK response

            return Ok(walkDTO);
        }

    }
}
