using FinanceManagement.Infrastructure.Models.Generated;
using Microsoft.EntityFrameworkCore;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories.Base
{
    public abstract class BaseRepository : IRepository, IDisposable
    {
        private GeneratedDbContext dbContext;

        public GeneratedDbContext DbContext
        {
            get => dbContext;
            set
            {
                dbContext?.Dispose();
                dbContext = value;
            }
        }

        public void Dispose()
        {
        }

        public async Task<T> GetById<T, K>(K id) where T : class
        {
            return await this.DbContext.Set<T>().FindAsync(id);
        }

        public void Insert<T>(T model) where T : class
        {
            this.DbContext.Set<T>().Add(model);
        }

        public void Remove<T>(T model) where T : class
        {
            this.DbContext.Set<T>().Remove(model);
        }

        public void Update<T>(T model) where T : class
        {
            this.DbContext.Set<T>().Update(model);
        }

        public void InsertRange<T>(List<T> models) where T : class
        {
            this.DbContext.Set<T>().AddRange(models);
        }

        public void UpdateRange<T>(List<T> models) where T : class
        {
            this.DbContext.Set<T>().UpdateRange(models);
        }

        public void RemoveRange<T>(List<T> models) where T : class
        {
            this.DbContext.Set<T>().RemoveRange(models);
        }

        public async Task InsertAndSaveAsync<T>(T model) where T : class
        {
            this.DbContext.Set<T>().Add(model);

            await this.DbContext.SaveChangesAsync();
        }

        public async Task InsertRangeAndSaveAsync<T>(List<T> models) where T : class
        {
            await this.DbContext.Set<T>().AddRangeAsync(models);

            await this.DbContext.SaveChangesAsync();
        }

        public async Task RemoveAndSaveAsync<T>(T model) where T : class
        {
            this.DbContext.Set<T>().Remove(model);

            await this.DbContext.SaveChangesAsync();
        }

        public async Task RemoveRangeAndSaveAsync<T>(List<T> models) where T : class
        {
            this.DbContext.Set<T>().RemoveRange(models);

            await this.DbContext.SaveChangesAsync();
        }

        public async Task UpdateAndSaveAsync<T>(T model) where T : class
        {
            this.DbContext.Set<T>().Update(model);

            await this.DbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAndSaveAsync<T>(List<T> models) where T : class
        {
            this.DbContext.Set<T>().UpdateRange(models);

            await this.DbContext.SaveChangesAsync();
        }

        public DbConnection DbConnection
        {
            get
            {
                if (DbContext == null) return null;

                return new ProfiledDbConnection(dbContext.Database.GetDbConnection(), MiniProfiler.Current);
            }
        }
    }

    public abstract class BaseRepository<T> : BaseRepository, IRepository, IDisposable where T : class
    {
        public DbSet<T> EntitiesTracked => DbContext.Set<T>();

        public IQueryable<T> Entities => DbContext.Set<T>().AsNoTracking();
    }
}
