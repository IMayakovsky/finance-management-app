using FinanceManagement.Infrastructure.Models.Generated;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories.Base
{
    public interface IRepository : IDisposable
    {
        GeneratedDbContext DbContext { get; set; }

        Task<T> GetById<T, K>(K id) where T : class;

        void Insert<T>(T model) where T : class;

        void Remove<T>(T model) where T : class;

        void Update<T>(T model) where T : class;

        void InsertRange<T>(List<T> models) where T : class;

        void UpdateRange<T>(List<T> models) where T : class;

        void RemoveRange<T>(List<T> models) where T : class;

        Task InsertAndSaveAsync<T>(T model) where T : class;

        Task InsertRangeAndSaveAsync<T>(List<T> model) where T : class;

        Task RemoveAndSaveAsync<T>(T model) where T : class;

        Task RemoveRangeAndSaveAsync<T>(List<T> models) where T : class;

        Task UpdateAndSaveAsync<T>(T model) where T : class;

        Task UpdateRangeAndSaveAsync<T>(List<T> models) where T : class;
    }
}
