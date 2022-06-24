using FinanceManagement.Infrastructure.Dto.Enums;

namespace FinanceManagement.Infrastructure.Dto
{
    public class NotificationRequestDto
    {
        public NotificationTypeEnum? NotificationType { get; set; }
        public int UserId { get; set; }
        public bool? IsRead { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
