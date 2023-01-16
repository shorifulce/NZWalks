using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.api.Repositories;

namespace NZWalks.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;



        // As IRegionRepository gives me regionRepository from the service definied program.cs 
        // I can get all the method of regionRepository class 
        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllWalkAync()
        {
            var walkDifficulties = await walkDifficultyRepository.GetAllAsync();
           

            var WalkDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);

            return Ok(WalkDifficultyDTO);

            // when I call GetAll() ,it will call the method of GetAll from RegionRepositoy class
            //ok means 200 success comming back from restapi

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);

            if (walkDifficulty == null)
            {
                return NotFound();
            }
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);


        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyDTOAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            // Request DTO to Domain Model

            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code,
               
            };


            // Pass details means Domain class info to Repository

            walkDifficulty = await walkDifficultyRepository.AddAsync(walkDifficulty);

            // Convert Back to DTO (Domain class to DTO for sending it client cz Domain class should not be outside)
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
               
                Code = walkDifficulty.Code,
                
            };

            //name of name of function, so name of this method and then we will pass the object value back. 
            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);

        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            //Get  region from Database

            var walkDifficulty = await walkDifficultyRepository.DeleteAsync(id);

            // iff null not found

            if (walkDifficulty == null)
            {
                return NotFound();
            }

            // convert response back to DTO

            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Id = walkDifficulty.Id,
                Code = walkDifficulty.Code,
                

            };

            // return OK response

            return Ok(walkDifficultyDTO);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficulty)
        {
            // Request DTO to Domain Model

            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficulty.Code,
               
            };


            // Pupdate region using Repository

            walkDifficulty = await walkDifficultyRepository.UpdateAsync(id, walkDifficulty);

            // if ull then notfound

            if (walkDifficulty == null)
            {
                return NotFound();
            }

            // Convert Domain to DTO 
            var walkDifficultyDTO = new Models.DTO.Region
            {
                Id = walkDifficulty.Id,
                Code = walkDifficulty.Code,
                
            };

            // return ok

            return Ok(walkDifficultyDTO);
        }
    
    }
}
