using System;
using System.Collections.Generic;

namespace FinanceManagement.Infrastructure.Dto
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? NextBilling { get; set; }
    }

    public class AccountSubscriptionsDto
    {
        public int AccountId { get; set; }
        public List<SubscriptionDto> Subscriptions { get; set; }
    }
}
