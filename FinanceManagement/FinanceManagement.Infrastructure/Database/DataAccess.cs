using FinanceManagement.Core.Exceptions;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Database
{
    /// <summary>
    /// Interface for accessing repositories and DbContexts.
    /// It creates single DbContext per instance of IDataAccess.
    /// </summary>
    public interface IDataAccess : IDisposable
    {
        /// <summary>
        /// Get any IRepository
        /// </summary>
        /// <typeparam name="T">Concrete type of the IRepository</typeparam>
        /// <returns></returns>
        T Repository<T>() where T : IRepository;

        GeneratedDbContext DbContext { get; }
        Task SaveDbContext();
    }

    /// <summary>
    /// This implementation is not thread safe!
    /// </summary>
    public class DataAccess : IDataAccess
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IDbContextFactory<GeneratedDbContext> dbContextFactory;

        private GeneratedDbContext dbContext;

        private Dictionary<Type, IRepository> repositoryCache;
        private List<IDisposable> toDispose;

        public DataAccess(IServiceProvider serviceProvider, IDbContextFactory<GeneratedDbContext> dbContextFactory)
        {
            this.serviceProvider = serviceProvider;
            this.dbContextFactory = dbContextFactory;

            this.repositoryCache = null;
            this.toDispose = null;
        }

        public GeneratedDbContext DbContext
        {
            get
            {
                if (dbContext == null)
                {
                    dbContext = dbContextFactory.CreateDbContext();

                    AddToDispose(dbContext);
                }

                return dbContext;
            }
        }

        public async Task SaveDbContext()
        {
            if (dbContext != null)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        public T Repository<T>() where T : IRepository
        {
            var cached = RetrieveCachedRepository<T>();
            if (cached != null)
            {
                return cached;
            }

            T newRepository = (T)serviceProvider.GetService(typeof(T));

            if (newRepository == null)
            {
                throw new BaseException($"{nameof(IRepository)} of type {typeof(T).Name} was created as null value.");
            }

            if (newRepository is IRepository platformRepository)
            {
                platformRepository.DbContext = DbContext;
            }

            CacheRepository<T>(newRepository);

            AddToDispose(newRepository);

            return newRepository;
        }

        public void Dispose()
        {
            foreach (IDisposable o in toDispose ?? Enumerable.Empty<IDisposable>())
            {
                o?.Dispose();
            }
        }

        private void AddToDispose(IDisposable disposable)
        {
            if (toDispose == null)
            {
                toDispose = new List<IDisposable>();
            }

            toDispose.Add(disposable);
        }

        private void CacheRepository<T>(IRepository repository) where T : IRepository
        {
            repositoryCache[typeof(T)] = repository;
        }

        private T RetrieveCachedRepository<T>() where T : IRepository
        {
            if (repositoryCache == null)
            {
                this.repositoryCache = new Dictionary<Type, IRepository>();
            }

            if (repositoryCache.TryGetValue(typeof(T), out IRepository repository))
            {
                return (T)repository;
            }

            return default;
        }
    }
}
