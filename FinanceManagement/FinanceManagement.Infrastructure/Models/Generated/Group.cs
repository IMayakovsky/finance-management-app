using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class Group
    {
        public Group()
        {
            UserGroupRoles = new HashSet<UserGroupRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<UserGroupRole> UserGroupRoles { get; set; }
    }
}
