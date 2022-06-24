using FinanceManagement.Infrastructure.Dto.Enums;
using System.Collections.Generic;

namespace FinanceManagement.Infrastructure.Dto
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public string Name { get; set; }
        public string Parameters { get; set; }
    }

    public class NotificationPageDto
    {
        public int TotalRowCount { get; set; }
        public List<NotificationDto> Notifications { get; set; }
    }
}
