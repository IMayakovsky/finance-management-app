using FinanceManagement.Infrastructure.Models;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface ITransactionRepository : IRepository
    {
        Task<Transaction> GetByIdAndUserId(int transactionId, int userId);
        Task<EntitiesWithCount<Transaction>> GetByAccountId(int account, int page, int pageSize);
    }

    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public async Task<Transaction> GetByIdAndUserId(int transactionId, int userId)
        {
            return await Entities.Where(e => e.Id == transactionId && e.Account.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<EntitiesWithCount<Transaction>> GetByAccountId(int accountId, int page, int pageSize)
        {
            var res = new EntitiesWithCount<Transaction>();
            var query = Entities.Where(e => e.AccountId == accountId);

            res.Count = query.Count();
            res.Entities = await query.OrderByDescending(t => t.Date).Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();

            return res;
        }
    }
}
