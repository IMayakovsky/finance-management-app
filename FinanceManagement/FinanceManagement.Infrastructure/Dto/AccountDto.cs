using FinanceManagement.Infrastructure.Dto.Enums;
using System;

namespace FinanceManagement.Infrastructure.Dto
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public CurrencyEnum Currency { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
    }
}
