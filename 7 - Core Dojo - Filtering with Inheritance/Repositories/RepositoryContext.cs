using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Configurations;
using Repositories.Entities;
using System;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;

namespace Repositories
{
    public partial class RepositoryContext : DbContext
    {
        public RepositoryContext() { }

        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }

        // List of Tables
        public virtual DbSet<Student> Students { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                var webProjectLocation = System.IO.Directory.GetParent(Environment.CurrentDirectory) + "\\Core Dojo";
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(webProjectLocation)
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new StudentConfigurations().Configure(modelBuilder.Entity<Student>());
        }
    }
}
