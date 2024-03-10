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

        public async Task<List<Walk>> GetAllAsync(
            string? filterOn = null, 
            string? filterQuery = null, 
            string? sortBy = null, 
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000)
        {
            // instead of a list we are converting the result set into a Queryable object
            // we can use Linq with the Querable object
            var walks = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            // Filter
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKM) : walks.OrderByDescending(x => x.LengthInKM);
                }
            }

            // Pagination
            /* 
             * let assume, pageNumber = 1 and pageSize = 10
             * skipResults = (1-1) * 10 = 0
             * if pageNumber = 2 and pageSize = 10
             * skipResults = (2-1) * 10 = 10
             * * this is important for Skip() and Take() which are below
             * so, if we Skip(0) and Take(10) it gives 1st 10 rows
             * if we Skip(10) and Take(10) it will give 2nd 10 rows and skip the 1st 10 rows 
             */
            var skipResults = (pageNumber - 1) * pageSize;  

            /* 
             * Skip() -> takes an integer and skip that no. of rows from the result
             * if we get 15 rows as result, when we use Skip(5) it will skip 5 and return only 10 rows as result
             * Take() -> takes an integer and only gives the given number of results
             * when we use this with Skip() it skips the 1st number of results and Take() the results next to them
             */
            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();

            //return await _dbContext.Walks.ToListAsync();
            // When we have Navigation properties (Database Relatioships) we need to include it.
            // Here, ["Difficulty" and "Region"] are other tables which are related to Walk table
            // Open, Walk.cs to get an idea
            // The below is using String type but, to make it type safe we can do it with action(x => x.Model)
            // -> prev_code = return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
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
