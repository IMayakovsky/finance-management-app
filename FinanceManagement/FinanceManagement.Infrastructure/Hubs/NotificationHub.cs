using FinanceManagement.Infrastructure.Dto;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Hubs
{
    public interface INotificationHub
    {
        Task SendNotification(NotificationDto notification);
    }

    public class NotificationHub : BaseHub<INotificationHub>
    {
    }
}
