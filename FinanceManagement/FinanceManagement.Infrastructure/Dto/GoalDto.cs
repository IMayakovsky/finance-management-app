using FinanceManagement.Infrastructure.Dto.Enums;
using System;

namespace FinanceManagement.Infrastructure.Dto
{
    public class GoalDto
    {
        public int Id { get; set; }
        public double CurrentAmount { get; set; }
        public double FullAmount { get; set; }
        public DateTime Created { get; set; }
        public DateTime? DateTo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public CurrencyEnum Currency { get; set; }
    }
}
