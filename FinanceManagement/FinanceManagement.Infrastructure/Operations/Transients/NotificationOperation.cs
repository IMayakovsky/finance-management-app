using FinanceManagement.Core.Exceptions;
using FinanceManagement.Core.Operations;
using FinanceManagement.Infrastructure.Database;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Hubs;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Operations.Base;
using FinanceManagement.Infrastructure.Repositories;
using Mapster;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Operations.Transients
{
    public interface INotificationOperation : ITransientOperation
    {
        Task<NotificationPageDto> GetNotifications(NotificationRequestDto request);
        Task ReadNotification(int notificationId, int userId);
        Task DeleteNotification(int notificationId, int userId);
        Task InsertNotification(Notification notification);
    }

    public class NotificationOperation : BaseInfrastructureOperation, INotificationOperation
    {
        private readonly IDataAccess dataAccess;
        private readonly IHubContext<NotificationHub, INotificationHub> notificationHubContext;

        public NotificationOperation(IDataAccess dataAccess, IHubContext<NotificationHub, INotificationHub> notificationHubContext)
        {
            this.dataAccess = dataAccess;
            this.notificationHubContext = notificationHubContext;
        }

        public async Task<NotificationPageDto> GetNotifications(NotificationRequestDto request)
        {
            var res = await dataAccess.Repository<INotificationRepository>().GetNotifications(request);

            return new NotificationPageDto
            {
                TotalRowCount = res.Count,
                Notifications = res.Entities.Select(u => u.Adapt<NotificationDto>()).ToList()
            };
        }

        public async Task InsertNotification(Notification notification)
        {
            await dataAccess.Repository<INotificationRepository>().InsertAndSaveAsync(notification);
        }

        public async Task ReadNotification(int notificationId, int userId)
        {
            var model = await dataAccess.Repository<INotificationRepository>().GetByIdAndUserId(notificationId, userId);

            if (model == null)
            {
                throw new NotFoundException($"Notification with id {notificationId} and UserId {userId} was not found");
            }

            model.IsRead = true;

            await dataAccess.Repository<INotificationRepository>().UpdateAndSaveAsync(model);
        }

        public async Task DeleteNotification(int notificationId, int userId)
        {
            var model = await dataAccess.Repository<INotificationRepository>().GetByIdAndUserId(notificationId, userId);

            if (model == null)
            {
                return;
            }

            await dataAccess.Repository<INotificationRepository>().RemoveAndSaveAsync(model);
        }
    }
}
