using FinanceManagement.Infrastructure.Database;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Operations.Transients;
using FinanceManagement.Infrastructure.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.ValidityInformer.Processors
{
    internal class SubscriptionBillingProccessor : BaseProccessor
    {
        private readonly ISubscriptionOperation subscriptionOperation;
        private readonly INotificationOperation notificationOperation;
        private readonly ITransactionOperation transactionOperation;
        private readonly IDataAccess dataAccess;

        public SubscriptionBillingProccessor(ISubscriptionOperation subscriptionOperation, 
            INotificationOperation notificationOperation, 
            ITransactionOperation transactionOperation, 
            IDataAccess dataAccess)
        {
            this.subscriptionOperation = subscriptionOperation;
            this.notificationOperation = notificationOperation;
            this.transactionOperation = transactionOperation;
            this.dataAccess = dataAccess;
        }

        public override async Task Process()
        {
            var subscriptions = await subscriptionOperation.GetSubscriptionsWithoutActualBill();

            if (!subscriptions.Any())
            {
                return;
            }

            List<SubscriptionsBill> bills = new List<SubscriptionsBill>();

            foreach (var sub in subscriptions)
            {
                TransactionDto transaction = new TransactionDto
                {
                    Amount = -sub.Amount,
                    Date = DateTime.UtcNow,
                    Name = sub.Name,
                };

                await transactionOperation.CreateTransaction(transaction, sub.UserId);

                Notification notification = new Notification
                {
                    NotificationTypeId = (int) NotificationTypeEnum.Information,
                    UserId = sub.UserId,
                    Name = NotificationNameEnum.SubcriptionBill.ToString(),
                    Parameters = JsonConvert.SerializeObject(new { Amount = sub.Amount, Name = sub.Name }),
                };

                bills.Add(new SubscriptionsBill
                {
                    Date = DateTime.UtcNow,
                    SubscriptionId = sub.Id,
                });

                await notificationOperation.InsertNotification(notification);
            }

            await dataAccess.Repository<ISubscriptionBillRepository>().InsertRangeAndSaveAsync(bills);
        }
    }
}
