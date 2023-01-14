using Microsoft.EntityFrameworkCore;
using NZWalks.api.Models.Domain;

namespace NZWalks.api.Data
{
    public class NZWalksDbContext:DbContext
    {

        // This contructor will ensure which db context will be context here
        // pass it to base class trough :base(options) 
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options):base(options)
        {
            // time to cretate data connection file here
        }

        public DbSet<Region> Regions{ get; set; }
        // this DbSet will tell entity framework that creat  a db table if it does not exists

        public DbSet<Walk> Walks { get; set; }

        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }
    }
}
