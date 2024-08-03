using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MyNewwRedis.Data.Configurations;
using MyNewwRedis.Models;
using StackExchange.Redis;
using System.Reflection.Emit;

namespace MyNewwRedis.Data
{
    public class DbContextClass : DbContext
    {
        public DbContextClass(DbContextOptions<DbContextClass> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

        }


    }
}

