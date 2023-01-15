using NZWalks.api.Models.Domain;

namespace NZWalks.api.Repositories
{
    public interface IRegionRepository
    {
        //We have made a tasks of regions 
       Task<IEnumerable<Region>> GetAllAsync();
    }
}
