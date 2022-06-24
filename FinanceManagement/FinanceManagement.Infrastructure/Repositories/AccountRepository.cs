using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface IAccountRepository : IRepository
    {
        Task<Account> GetByIdAndUserId(int AccountId, int userId);
        Task<List<Account>> GetByUserId(int userId);
    }

    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public async Task<Account> GetByIdAndUserId(int accountId, int userId)
        {
            return await Entities.Where(e => e.Id == accountId && e.UserId == userId && !e.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<List<Account>> GetByUserId(int userId)
        {
            return await Entities.Where(e => e.UserId == userId).ToListAsync();
        }
    }
}
