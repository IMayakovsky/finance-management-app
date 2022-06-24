using FinanceManagement.Infrastructure.Dto.Enums;
using System;

namespace FinanceManagement.Infrastructure.Dto.Groups
{
    public class UserGroupRolesDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public GroupRoleEnum Role { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
