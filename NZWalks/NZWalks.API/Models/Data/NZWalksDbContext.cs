using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.Data
{
    public class NZWalksDbContext: DbContext // we are inheriting DbContext
    {
        // setting DbContextOptions: it is used because we'll configure our Database connection in Program.cs
        // we are passing the options object to base class constructor as well [base(dbContextOptions)]
        public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }
        /* The following properties are used to specify the tables or it will create the tables accordingly 
        to the database when we migrate */
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
    }
}
