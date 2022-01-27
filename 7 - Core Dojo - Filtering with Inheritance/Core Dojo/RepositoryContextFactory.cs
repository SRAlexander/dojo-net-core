using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Dojo
{
    class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public IConfiguration _configuration { get; private set; }

        public RepositoryContextFactory() { }

        public RepositoryContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RepositoryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Repositories"));

            return new RepositoryContext(optionsBuilder.Options);
        }
    }
}