using FinanceManagement.Infrastructure.Dto.Enums;

namespace FinanceManagement.Infrastructure.Dto.Groups
{
    public class GroupCreateRequest
    { 
        public string GroupName { get; set; }
        public CurrencyEnum Currency { get; set; }
    }
}
