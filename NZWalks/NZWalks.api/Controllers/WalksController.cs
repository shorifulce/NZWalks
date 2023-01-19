using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.api.Models.DTO;
using NZWalks.api.Repositories;

namespace NZWalks.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalksRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public IRegionRepository regionRepository { get; }

        public WalksController(IWalksRepository walkRepository, IMapper mapper,IRegionRepository regionRepository,IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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
            // Normal validation is good but we can use both normal and fluent validation

            //validate the incomeing request
            if(!(await ValidateAddWalkAysnc(addWalksRequest)))
            {
                return BadRequest(ModelState);
            }

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

            // 

            //validate the incomeing request
            if (!(await ValidateUpdateWalkAsync(updateWalksRequest)))
            {
                return BadRequest(ModelState);
            }


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

        private async Task<bool> ValidateAddWalkAysnc(Models.DTO.AddWalksRequest addWalksRequest)
        {
            if (addWalksRequest == null)
            {
                ModelState.AddModelError(nameof(addWalksRequest), $"Add Region data is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalksRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalksRequest.Name), $"{nameof(addWalksRequest.Name)} can not be null or empty or white pace. ");

            }

            if (addWalksRequest.Length < 0)
            {
                ModelState.AddModelError(nameof(addWalksRequest.Length), $"{nameof(addWalksRequest.Length)} Should be greater than zero. ");

            }

            var region =await regionRepository.GetAsync(addWalksRequest.RegionId);
            
            if(region == null)
            {
                ModelState.AddModelError(nameof(addWalksRequest.RegionId), $"{nameof(addWalksRequest.RegionId)} Is invalid  ");

            }

            var waldifficulty = await walkDifficultyRepository.GetAsync(addWalksRequest.WalkDifficultyID);


            if(waldifficulty == null) 
            {
                ModelState.AddModelError(nameof(addWalksRequest.WalkDifficultyID), $"{nameof(addWalksRequest.WalkDifficultyID)} Is invalid ");

            }


            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;

        }

        #region Private methods
        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalksRequest updateWalksRequest)
        {
            if (updateWalksRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalksRequest), $"Add walk data is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalksRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalksRequest.Name), $"{nameof(updateWalksRequest.Name)} can not be null or empty or white pace. ");

            }

            if (updateWalksRequest.Length < 0)
            {
                ModelState.AddModelError(nameof(updateWalksRequest.Length), $"{nameof(updateWalksRequest.Length)} Should be greater than zero. ");

            }

            var region = await regionRepository.GetAsync(updateWalksRequest.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalksRequest.RegionId), $"{nameof(updateWalksRequest.RegionId)} Is invalid  ");

            }

            var waldifficulty = await walkDifficultyRepository.GetAsync(updateWalksRequest.WalkDifficultyID);


            if (waldifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalksRequest.WalkDifficultyID), $"{nameof(updateWalksRequest.WalkDifficultyID)} Is invalid ");

            }


            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;

        }
        #endregion

    }
}
