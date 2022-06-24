using FinanceManagement.Infrastructure.Attributes;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Operations.Transients;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManagement.Web.Controllers
{
    /// <summary>
    /// Controller for Notifications
    /// </summary>
    [BaseAuthorize]
    [Route("api/notifications")]
    public class NotificationController : BaseController
    {
        private readonly Lazy<INotificationOperation> notificationOperation;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="notificationOperation"></param>
        public NotificationController(Lazy<INotificationOperation> notificationOperation)
        {
            this.notificationOperation = notificationOperation;
        }

        /// <summary>
        /// Returns User's Notifications
        /// </summary>
        /// <returns></returns>
        [HttpPost("getNotifications")]
        public async Task<NotificationPageDto> GetUserNotifications(NotificationRequestDto request)
        {
            request.UserId = userId;
            return await notificationOperation.Value.GetNotifications(request);
        }

        /// <summary>
        /// Deletes notification
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        [HttpDelete("{notificationId}")]
        public async Task DeleteNotification(int notificationId)
        {
            await notificationOperation.Value.DeleteNotification(notificationId, userId);
        }

        /// <summary>
        /// Updates Notification
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        [HttpPatch("{notificationId}/read")]
        public async Task ReadNotification(int notificationId)
        {
            await notificationOperation.Value.ReadNotification(notificationId, userId);
        }
    }
}
