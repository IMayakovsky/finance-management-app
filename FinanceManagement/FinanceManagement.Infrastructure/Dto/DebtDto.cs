using FinanceManagement.Infrastructure.Dto.Enums;
using System;

namespace FinanceManagement.Infrastructure.Dto
{
    public class DebtDto
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public DateTime? DueTo { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
        public CurrencyEnum Currency { get; set; }
    }
}
