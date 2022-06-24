using FinanceManagement.Infrastructure.Dto.Groups;
using System.Collections.Generic;

namespace FinanceManagement.Infrastructure.Dto.Dashboards
{
    public class DashboardDataDto
    {
        public List<TransactionDto> Transactions { get; set; }
        public List<DebtDto> NegativeDebts { get; set; }
        public List<DebtDto> PositiveDebts { get; set; }
        public List<SubscriptionDto> Subscriptions { get; set; }
        public List<GoalDto> Goals { get; set; }
        public List<GroupDto> Groups { get; set; }
    }
}
