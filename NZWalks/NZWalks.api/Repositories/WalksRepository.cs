using Microsoft.EntityFrameworkCore;
using NZWalks.api.Data;
using NZWalks.api.Models.Domain;

namespace NZWalks.api.Repositories
{
    public class WalksRepository : IWalksRepository
    {

        private readonly NZWalksDbContext nZWalksDbContext;

        public WalksRepository(NZWalksDbContext nZWalksDbContext)
        {

            this.nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var walk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (walk == null)
            {
                return null;
            }

            // Delete the region
            nZWalksDbContext.Walks.Remove(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;

        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await nZWalksDbContext.Walks.Include(x=>x.Regin).Include(x=>x.WalkDifficulty).ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Walks.Include(x => x.Regin).Include(x => x.WalkDifficulty).FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await nZWalksDbContext.Walks.Include(x => x.Regin).Include(x => x.WalkDifficulty).FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Length = walk.Length;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.WalkDifficultyID = walk.WalkDifficultyID;
            

            await nZWalksDbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
