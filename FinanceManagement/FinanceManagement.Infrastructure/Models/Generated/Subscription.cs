using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class Subscription
    {
        public Subscription()
        {
            SubscriptionsBills = new HashSet<SubscriptionsBill>();
        }

        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Name { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<SubscriptionsBill> SubscriptionsBills { get; set; }
    }
}
