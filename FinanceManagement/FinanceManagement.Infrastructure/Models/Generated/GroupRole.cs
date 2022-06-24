using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class GroupRole
    {
        public GroupRole()
        {
            UserGroupRoles = new HashSet<UserGroupRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserGroupRole> UserGroupRoles { get; set; }
    }
}
