using NZWalks.api.Data;
using NZWalks.api.Models.Domain;

namespace NZWalks.api.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        //using ctrl+. we can create this attribute from constructor instance
        private readonly NZWalksDbContext nZWalksDbContext;

        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
         
            this.nZWalksDbContext = nZWalksDbContext;
        }


        public IEnumerable<Region> GetAll()
        {
           return nZWalksDbContext.Regions.ToList();
        }
    }
}
