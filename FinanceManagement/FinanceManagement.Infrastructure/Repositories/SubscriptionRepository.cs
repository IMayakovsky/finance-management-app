using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface ISubscriptionRepository : IRepository
    {
        Task<Subscription> GetByIdAndAccountId(int subscriptionId, int userId);
        Task<List<Subscription>> GetByAccountIdsIncludesAccount(List<int> accountIds);
        Task<List<Subscription>> GetExpiredSubscriptionsWithAccount(DateTime maxDateTime);
        Task<List<Subscription>> GetWithoutActualBillWithAccount(DateTime maxDateTime);
    }

    public class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
    {
        public async Task<Subscription> GetByIdAndAccountId(int subscriptionId, int accountId)
        {
            return await Entities.Where(e => e.Id == subscriptionId && e.AccountId == accountId).FirstOrDefaultAsync();
        }

        public async Task<List<Subscription>> GetByAccountIdsIncludesAccount(List<int> accountIds)
        {
            return await Entities.Where(e => accountIds.Contains(e.AccountId)).Include(e => e.Account).ToListAsync();
        }

        public async Task<List<Subscription>> GetExpiredSubscriptionsWithAccount(DateTime maxDateTime)
        {
            return await Entities.Where(e => e.DateTo < maxDateTime).Include(e => e.Account).ToListAsync();
        }

        public async Task<List<Subscription>> GetWithoutActualBillWithAccount(DateTime maxDateTime)
        {
            return await Entities.Where(e => !e.SubscriptionsBills.Any(cs => cs.Date >= maxDateTime) && !e.Account.IsDeleted).Include(e => e.Account).ToListAsync();
        }
    }
}
