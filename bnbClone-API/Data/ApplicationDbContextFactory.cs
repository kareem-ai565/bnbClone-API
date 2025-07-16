using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace bnbClone_API.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Build configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Set to root of project
                .AddJsonFile("appsettings.json")
                .Build();

            // Get connection string
            var connectionString = config.GetConnectionString("DefaultConnection");

            // Configure options
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
