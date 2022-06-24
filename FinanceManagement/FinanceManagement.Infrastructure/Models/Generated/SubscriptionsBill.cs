using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class SubscriptionsBill
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int SubscriptionId { get; set; }

        public virtual Subscription Subscription { get; set; }
    }
}
