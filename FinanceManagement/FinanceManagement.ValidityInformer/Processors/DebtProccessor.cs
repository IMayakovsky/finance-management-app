using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Operations.Transients;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FinanceManagement.ValidityInformer.Processors
{
    internal class DebtProccessor : BaseProccessor
    {
        private readonly IDebtOperation debtOperation;
        private readonly INotificationOperation notificationOperation;

        public DebtProccessor(IDebtOperation debtOperation, INotificationOperation notificationOperation)
        {
            this.debtOperation = debtOperation;
            this.notificationOperation = notificationOperation;
        }

        public override async Task Process()
        {
            var expired = await debtOperation.GetExpiredDebts(Constants.DebtNotificationPerioDays);

            foreach (var debt in expired)
            {
                NotificationParams parameters = new NotificationParams
                {
                    DueTo = debt.DueTo.Value,
                    Amount = debt.Amount,
                    Name = debt.Name
                };

                Notification notification = new Notification
                {
                    NotificationTypeId = (int) NotificationTypeEnum.Validation,
                    UserId = debt.UserId,
                    Name = NotificationNameEnum.DebtExpired.ToString(),
                    Parameters = JsonConvert.SerializeObject(parameters),
                };

                await notificationOperation.InsertNotification(notification);
            }
        }

        private class NotificationParams
        {
            public DateTime DueTo { get; set; }
            public double Amount { get; set; }
            public string Name { get; set; }
        }
    }
}
