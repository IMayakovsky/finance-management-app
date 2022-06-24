using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class Goal
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
        public string Currency { get; set; }

        public virtual User User { get; set; }
    }
}
