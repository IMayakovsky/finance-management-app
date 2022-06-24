using FinanceManagement.Core.Operations;
using FinanceManagement.Infrastructure.Dto.Dashboards;
using FinanceManagement.Infrastructure.Helpers;
using FinanceManagement.Infrastructure.Operations.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Operations.Transients
{
    public interface IDashboardOperation : ITransientOperation
    {
        Task<DashboardDataDto> GetTopEntities(int userId, int accountId, int count);
    }

    public class DashboardOperation : BaseInfrastructureOperation, IDashboardOperation
    {
        private readonly Lazy<ISubscriptionOperation> subscriptionOperation;
        private readonly Lazy<IDebtOperation> debtOperation;
        private readonly Lazy<IGoalOperation> goalOperation;
        private readonly Lazy<IGroupOperation> groupOperation;
        private readonly Lazy<ITransactionOperation> transactionOperation;
        private readonly Lazy<ISecurityHelper> securityHelper;

        public DashboardOperation(Lazy<ISubscriptionOperation> subscriptionOperation,
            Lazy<IDebtOperation> debtOperation,
            Lazy<IGoalOperation> goalOperation,
            Lazy<IGroupOperation> groupOperation,
            Lazy<ITransactionOperation> transactionOperation, 
            Lazy<ISecurityHelper> securityHelper)
        {
            this.subscriptionOperation = subscriptionOperation;
            this.debtOperation = debtOperation;
            this.goalOperation = goalOperation;
            this.groupOperation = groupOperation;
            this.transactionOperation = transactionOperation;
            this.securityHelper = securityHelper;
        }

        public async Task<DashboardDataDto> GetTopEntities(int userId, int accountId, int count)
        {
            DashboardDataDto result = new DashboardDataDto();

            await securityHelper.Value.CheckUserAccountPermissions(userId, accountId);

            var debts = (await debtOperation.Value.GetUserDebtsCached(userId)).OrderByDescending(e => e.DueTo);

            result.Transactions = await transactionOperation.Value.GetLastAccountTransactionsCached(accountId);
            result.Subscriptions = (await subscriptionOperation.Value.GetAccountSubscriptionsCached(new List<int> { accountId })).Take(count).ToList();
            result.PositiveDebts = debts.Where(d => d.Amount > 0).Take(count).ToList();
            result.NegativeDebts = debts.Where(d => d.Amount < 0).Take(count).ToList();
            result.Goals = (await goalOperation.Value.GetUserGoalsCached(userId)).OrderByDescending(e => e.DateTo).Take(count).ToList();
            result.Goals = (await goalOperation.Value.GetUserGoalsCached(userId)).OrderByDescending(e => e.DateTo).Take(count).ToList();
            result.Groups = (await groupOperation.Value.GetUserGroupsCached(userId)).Take(count).ToList();

            return result;
        }

        
    }
}
