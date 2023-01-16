using NZWalks.api.Models.Domain;

namespace NZWalks.api.Repositories
{
    public interface IWalkDifficultyRepository
    {
        //We have made a tasks of WalkDifficulty 
        Task<IEnumerable<WalkDifficulty>> GetAllAsync();

        Task<WalkDifficulty> GetAsync(Guid id);

        Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty);

        Task<WalkDifficulty> DeleteAsync(Guid id);

        Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty);
    }
}
