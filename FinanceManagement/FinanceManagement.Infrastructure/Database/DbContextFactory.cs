using FinanceManagement.Infrastructure.Models.Generated;
using Microsoft.EntityFrameworkCore;
using System;

namespace FinanceManagement.Infrastructure.Database
{
    public class DbContextFactory : IDbContextFactory<GeneratedDbContext>
    {
        private readonly Action<DbContextOptionsBuilder> defaultDbContextOptions;

        public DbContextFactory(Action<DbContextOptionsBuilder> defaultDbContextOptions)
        {
            this.defaultDbContextOptions = defaultDbContextOptions;
        }

        public GeneratedDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GeneratedDbContext>();
            defaultDbContextOptions(optionsBuilder);

            return new GeneratedDbContext(optionsBuilder.Options);
        }
    }
}
