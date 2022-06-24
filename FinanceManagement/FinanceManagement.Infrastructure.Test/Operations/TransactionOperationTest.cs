using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Operations.Transients;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FinanceManagement.Infrastructure.Test.Operations
{
    public class TransactionOperationTest : BaseUnitTest
    {
        private readonly int accountId = 1;
        private readonly int userId = 1;
        private readonly int transactionAmount = 500;
        private readonly int transactionUpdatedAmount = 100;
        private readonly string transactionName = "Test Transaction";

        [Fact]
        public async Task CreateUpdateDeleteTransactionTest()
    {
            using (var scope = Services.CreateScope())
            {
                var transactionOperation = scope.ServiceProvider.GetService<ITransactionOperation>();
                var accountOperation = scope.ServiceProvider.GetService<IAccountOperation>();

                var account = await accountOperation.GetAccountByIdAndUserIdCached(accountId, userId);

                Assert.NotNull(account);

                var initAccountAmount = account.Amount;

                var transactionToCreate = new TransactionDto
                {
                    AccountId = accountId,
                    Amount = transactionAmount,
                    Date = DateTime.UtcNow,
                    Name = transactionName,
                };

                var transaction = await transactionOperation.CreateTransaction(transactionToCreate, userId);

                // Transaction was actual created
                Assert.True(transaction.Id > 0);
                // Created Transaction has the same properties
                Assert.Equal(transactionToCreate.Name, transaction.Name);
                Assert.Equal(transactionToCreate.Amount, transaction.Amount);

                account = await accountOperation.GetAccountByIdAndUserIdCached(accountId, userId);
                // Account's Amount was updated
                Assert.Equal(initAccountAmount + transaction.Amount, account.Amount);

                transaction.Amount -= transactionUpdatedAmount;
                await transactionOperation.UpdateTransaction(transaction, userId);
                transaction = await transactionOperation.GetTransactionByUserIdAndId(userId, transaction.Id);
                Assert.Equal(transactionToCreate.Amount - transactionUpdatedAmount, transaction.Amount);

                account = await accountOperation.GetAccountByIdAndUserIdCached(accountId, userId);
                // Account's Amount was updated
                Assert.Equal(initAccountAmount + transaction.Amount, account.Amount);


                transactionOperation = scope.ServiceProvider.GetService<ITransactionOperation>();

                await transactionOperation.DeleteTransaction(transaction.Id, userId);
                account = await accountOperation.GetAccountByIdAndUserIdCached(accountId, userId);
                // Account's Amount was updated
                Assert.Equal(initAccountAmount, account.Amount);
            }
        }
    }
}
