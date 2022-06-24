using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class Account
    {
        public Account()
        {
            Groups = new HashSet<Group>();
            Subscriptions = new HashSet<Subscription>();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public int? UserId { get; set; }
        public double Amount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
