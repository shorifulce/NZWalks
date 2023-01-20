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

        // we have to use this override method if we add new table and make relationship with them
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.Role)
                .WithMany(x => x.User_Roles)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.User)
                .WithMany(x=>x.User_Roles)
                .HasForeignKey(x => x.UserId);


        }

        public DbSet<Region> Regions{ get; set; }
        // this DbSet will tell entity framework that creat  a db table if it does not exists

        public DbSet<Walk> Walks { get; set; }

        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User_Role> User_Roles { get; set; }



    }
}
