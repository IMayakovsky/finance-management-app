using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class UserGroupRole
    {
        public int Id { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public int GroupRoleId { get; set; }

        public virtual Group Group { get; set; }
        public virtual GroupRole GroupRole { get; set; }
        public virtual User User { get; set; }
    }
}
