using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _dbContext;
        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            //return await _dbContext.Walks.ToListAsync();
            // When we have Navigation properties (Database Relatioships) we need to include it.
            // Here, ["Difficulty" and "Region"] are other tables which are related to Walk table
            // Open, Walk.cs to get an idea
            // The below is using String type but, to make it type safe we can do it with action(x => x.Model)
            return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
            // we can also use this method to make it type safe
            /* return await _dbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).ToListAsync(); */
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKM = walk.LengthInKM;
            existingWalk.WalkImageURL = walk.WalkImageURL;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await _dbContext.SaveChangesAsync();

            return existingWalk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk == null )
            {
                return null;
            }

            _dbContext.Remove(existingWalk);
            await _dbContext.SaveChangesAsync();

            return existingWalk;
        }
    }
}
