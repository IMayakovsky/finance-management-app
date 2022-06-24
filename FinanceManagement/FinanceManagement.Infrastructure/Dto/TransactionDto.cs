using System;
using System.Collections.Generic;

namespace FinanceManagement.Infrastructure.Dto
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public int AccountId { get; set; }
    }

    public class TransactionPageDto
    {
        public int TotalRowCount { get; set; }
        public List<TransactionDto> Transactions { get; set; }
    }
}
