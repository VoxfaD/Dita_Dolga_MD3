using MD3t.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MD3t.DB
{
    public class SchoolDbContext : DbContext   // es palaidu package manager MD3.DB komandas: Add-Migration InitialCreate, Update-Database, kas izveido datubāzi ar Code First pieeju
    {
        // tiek sagatavotas tabulas dati
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Assignement> Assignements { get; set; }
        public DbSet<Submission> Submissions { get; set; }

        private readonly IConfiguration _configuration;

        // tas ir domāts lai pareizi nostrādātu connection string
        public SchoolDbContext(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("DB");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DB' is not configured.");
            }

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API configuration can go here if needed
        }
    }
}
