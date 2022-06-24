using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class Debt
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime Created { get; set; }
        public DateTime? DueTo { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
        public bool IsClosed { get; set; }
        public string Currency { get; set; }

        public virtual User User { get; set; }
    }
}
