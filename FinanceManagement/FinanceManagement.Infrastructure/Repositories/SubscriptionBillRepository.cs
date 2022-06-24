using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface ISubscriptionBillRepository : IRepository
    {
    }

    public class SubscriptionBillRepository : BaseRepository<SubscriptionsBill>, ISubscriptionBillRepository
    {
    }
}
