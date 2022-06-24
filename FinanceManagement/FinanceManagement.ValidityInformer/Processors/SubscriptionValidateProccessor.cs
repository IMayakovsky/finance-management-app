using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Operations.Transients;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FinanceManagement.ValidityInformer.Processors
{
    internal class SubscriptionValidateProccessor : BaseProccessor
    {
        private readonly ISubscriptionOperation subscriptionOperation;
        private readonly INotificationOperation notificationOperation;

        public SubscriptionValidateProccessor(ISubscriptionOperation subscriptionOperation, INotificationOperation notificationOperation)
        {
            this.subscriptionOperation = subscriptionOperation;
            this.notificationOperation = notificationOperation;
        }

        public override async Task Process()
        {
            var expired = await subscriptionOperation.GetExpiredSubscriptions(Constants.SubscriptionNotificationPerioDays);

            foreach (var sub in expired)
            {
                NotificationParams parameters = new NotificationParams
                {
                    DateTo = sub.DateTo,
                    Amount = sub.Amount
                };

                Notification notification = new Notification
                {
                    NotificationTypeId = (int)NotificationTypeEnum.Validation,
                    UserId = sub.UserId,
                    Name = NotificationNameEnum.SubcriptionExpired.ToString(),
                    Parameters = JsonConvert.SerializeObject(parameters),
                };

                await notificationOperation.InsertNotification(notification);
            }
        }

        private class NotificationParams
        {
            public DateTime DateTo { get; set; }
            public double Amount { get; set; }
        }
    }
}
