using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public int AccountId { get; set; }
        public int? CategoryId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Category Category { get; set; }
    }
}
