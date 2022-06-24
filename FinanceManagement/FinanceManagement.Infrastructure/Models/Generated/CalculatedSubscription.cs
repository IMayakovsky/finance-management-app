using System;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class CalculatedSubscription
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int SubscriptionId { get; set; }

        public virtual Subscription Subscription { get; set; }
    }
}
