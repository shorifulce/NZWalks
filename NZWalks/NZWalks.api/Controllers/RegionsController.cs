using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NZWalks.api.Models.Domain;
using NZWalks.api.Models.DTO;
using NZWalks.api.Repositories;

namespace NZWalks.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;



        // As IRegionRepository gives me regionRepository from the service definied program.cs 
        // I can get all the method of regionRepository class 
        public RegionsController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAync()
        {
            var regions=await regionRepository.GetAllAsync();
            // return DTo regions
            //var regionsDTO = new List<Models.DTO.Region>();

            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTo = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    regionsDTO.Add(regionDTo);
            //});

            // all regions come from regionRepository.GetAll()
            // we are maiping regions to DTO

            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);

            // when I call GetAll() ,it will call the method of GetAll from RegionRepositoy class
            //ok means 200 success comming back from restapi

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            
            if(region == null)
            {
                return NotFound();  
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);


        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // Validation the request
            // if validation fails it means ! 

            if(!ValidateAddRegionAysnc(addRegionRequest))
            {
                return BadRequest();
            }

            // Request DTO to Domain Model

            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,
            };


            // Pass details means Domain class info to Repository

             region = await regionRepository.AddAsync(region);

            // Convert Back to DTO (Domain class to DTO for sending it client cz Domain class should not be outside)
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };

            //name of name of function, so name of this method and then we will pass the object value back. 
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
            
        }


        [HttpDelete]
        [Route("{id:guid}")] 
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get  region from Database

            var region=await regionRepository.DeleteAsync(id);

            // iff null not found

            if (region == null)
            {
                return NotFound();
            }

            // convert response back to DTO

            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,

            };

            // return OK response

            return Ok(regionDTO);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute ]Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            // Request DTO to Domain Model

            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };


            // Pupdate region using Repository

            region = await regionRepository.UpdateAsync(id,region);

            // if ull then notfound

            if(region==null)
            {
                return NotFound();
            }

            // Convert Domain to DTO 
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };

            // return ok

            return Ok(regionDTO);
        }


        #region Private methods

        private Boolean ValidateAddRegionAysnc(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if(addRegionRequest ==null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), $"Add Region data is required.");
                return false;
            }

            if(string.IsNullOrWhiteSpace(addRegionRequest.Code)) 
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),$"{nameof(addRegionRequest.Code)} can not be null or empty or white pace. ");
           
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} can not be null or empty or white pace. ");

            }

            if (addRegionRequest.Area <=0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{nameof(addRegionRequest.Area)} can not be less than or equal to zero. ");

            }


            if (addRegionRequest.Lat<=0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat), $"{nameof(addRegionRequest.Lat)} can not be less than or equal to zero. ");

            }

            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long), $"{nameof(addRegionRequest.Long)} can not be less than or equal to zero.");

            }
            if (addRegionRequest.Population <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population), $"{nameof(addRegionRequest.Population)} can not be less than or equal to zero.");

            }

            if(ModelState.ErrorCount>0)
            {
                return false;
            }

            return true;

        }

        #endregion

    }
}
