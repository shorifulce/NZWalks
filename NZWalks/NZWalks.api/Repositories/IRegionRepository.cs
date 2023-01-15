using NZWalks.api.Models.Domain;

namespace NZWalks.api.Repositories
{
    public interface IRegionRepository
    {
       IEnumerable<Region> GetAll();
    }
}
