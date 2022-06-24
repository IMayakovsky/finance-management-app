using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Models;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface INotificationRepository : IRepository
    {
        Task<Notification> GetById(int notificationId);
        Task<EntitiesWithCount<Notification>> GetNotifications(NotificationRequestDto request);
        Task<Notification> GetByIdAndUserId(int notificationId, int userId);
    }

    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public async Task<Notification> GetById(int notificationId)
        {
            return await Entities.Where(e => e.Id == notificationId).FirstOrDefaultAsync();
        }

        public async Task<EntitiesWithCount<Notification>> GetNotifications(NotificationRequestDto request)
        {
            int? type = request.NotificationType.HasValue ? (int?)request.NotificationType.Value : null;

            var query = Entities.Where(e => e.UserId == request.UserId
                && (!request.IsRead.HasValue || e.IsRead == request.IsRead)
                && (!type.HasValue || e.NotificationTypeId == type));

            return new EntitiesWithCount<Notification>
            {
                Count = query.Count(),
                Entities = await query.Skip(request.PageSize * (request.CurrentPage - 1)).Take(request.PageSize).ToListAsync()
            };
        }

        public async Task<Notification> GetByIdAndUserId(int notificationId, int userId)
        {
            return await Entities.Where(e => e.UserId == userId && e.Id == notificationId).FirstOrDefaultAsync();
        }
    }
}
