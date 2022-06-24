using System.Collections.Generic;
using System.Linq;

namespace FinanceManagement.Infrastructure.Dto.Groups
{
    public class UserGroupsRolesDto
    {
        public int UserId { get; set; }
        /// <summary>
        /// Key: groupId, Value: UserGroupRoles.
        /// </summary>
        public Dictionary<int, List<UserGroupRolesDto>> Roles { get; set; }
        public List<int> GroupIds => Roles.Keys.ToList();
    }
}
