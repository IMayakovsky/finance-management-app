using FinanceManagement.Infrastructure.Dto.Enums;
using System.Collections.Generic;

namespace FinanceManagement.Infrastructure.Dto.Groups
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AccountId { get; set; }
        public double Amount { get; set; }
        public CurrencyEnum Currency { get; set; }
        public List<UserGroupRolesDto> Roles { get; set; }
    }
}
