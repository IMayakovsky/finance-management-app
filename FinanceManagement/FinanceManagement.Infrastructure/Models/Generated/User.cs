using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class User
    {
        public User()
        {
            Accounts = new HashSet<Account>();
            Categories = new HashSet<Category>();
            Debts = new HashSet<Debt>();
            Goals = new HashSet<Goal>();
            Notifications = new HashSet<Notification>();
            RefreshTokens = new HashSet<RefreshToken>();
            RestorePasswords = new HashSet<RestorePassword>();
            UserGroupRoles = new HashSet<UserGroupRole>();
        }

        public int Id { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public bool IsAdmin { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Debt> Debts { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<RestorePassword> RestorePasswords { get; set; }
        public virtual ICollection<UserGroupRole> UserGroupRoles { get; set; }
    }
}
