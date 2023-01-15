using NZWalks.api.Models.Domain;


namespace NZWalks.api.Repositories
{
    public interface IRegionRepository
    {
        //We have made a tasks of regions 
       Task<IEnumerable<Region>> GetAllAsync();

        Task<Region> GetAsync(Guid id);

        Task<Region> AddAsync(Region region);

        Task<Region> DeleteAsync(Guid id);   
        
        Task<Region> UpdateAsync(Guid id,Region region);    
    }
}
