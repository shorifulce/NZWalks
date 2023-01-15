using Microsoft.AspNetCore.Mvc;
using NZWalks.api.Models.Domain;

namespace NZWalks.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        [HttpGet]
        public IActionResult GetAllRegions()
        {


            var regions = new List<Region>()
            {
                new Region
                {
                    Id=Guid.NewGuid(),
                    Name="Wllington",
                    Code="WLG",
                    Area=22144,
                    Lat=-1.8822,
                    Long=2999.88,
                    Population=50000

                },
                new Region
                {
                    Id=Guid.NewGuid(),
                    Name="Auckland",
                    Code="AuUCK",
                    Area=227755,
                    Lat=-1.8822,
                    Long=2999.88,
                    Population=50000

                },
            };
           
            return Ok(regions);  
            //ok means 200 success comming back from restapi

        }


    }
}
