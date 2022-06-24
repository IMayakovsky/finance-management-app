using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class RestorePassword
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserResetId { get; set; }
        public DateTime Expired { get; set; }

        public virtual User User { get; set; }
    }
}
