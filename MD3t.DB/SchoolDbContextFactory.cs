using MD3t.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MD3t.DB
{
    public class SchoolDbContextFactory : IDesignTimeDbContextFactory<SchoolDbContext> // connection string apstrāde
    {
        public SchoolDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SchoolDbContext>();
            string connectionString = configuration.GetConnectionString("DB");

            optionsBuilder.UseSqlServer(connectionString);

            return new SchoolDbContext(configuration);
        }
    }
}
