using System.Configuration;
using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.Entities.Concrete;

namespace Northwind.DataAccess.Concrete.EntityFramework.Contexts
{
    public class NorthwindContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
             //.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)  .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
            Console.WriteLine("***********"+configuration["ConnectionStrings"]);
            Console.WriteLine("***********"+environment);
           // Console.WriteLine("***********"+environment);

            optionsBuilder.UseSqlServer(configuration["ConnectionStrings"]);

         }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

    }
}