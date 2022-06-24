using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class Notification
    {
        public int Id { get; set; }
        public string Parameters { get; set; }
        public string Name { get; set; }
        public bool IsRead { get; set; }
        public int NotificationTypeId { get; set; }
        public int UserId { get; set; }

        public virtual NotificationType NotificationType { get; set; }
        public virtual User User { get; set; }
    }
}
