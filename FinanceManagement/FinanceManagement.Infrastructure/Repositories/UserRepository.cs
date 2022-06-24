using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task<List<int>> GetUserIdsPage(int count, int skip);
        Task<User> GetByEmail(string email);
        Task<List<User>> GetByIds(List<int> ids);
        Task<User> GetById(int id);
        Task<List<User>> GetUsers(string name, int currentPage, int pageSize);
    }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public async Task<List<int>> GetUserIdsPage(int count, int skip)
        {
            return await this.Entities
                .OrderBy(s => s.Id)
                .Select(s => s.Id)
                .Skip(skip)
                .Take(count)
                .ToListAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await Entities.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<List<User>> GetByIds(List<int> ids)
        {
            return await Entities.Where(e => ids.Contains(e.Id)).ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await Entities.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<User>> GetUsers(string name, int currentPage, int pageSize)
        {
            var query = string.IsNullOrEmpty(name) ? Entities : Entities.Where(e => e.Name.ToLower().Contains(name.ToLower()));

            return await query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
