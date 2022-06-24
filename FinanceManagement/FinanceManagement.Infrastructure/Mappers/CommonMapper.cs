using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Dto.Groups;
using FinanceManagement.Infrastructure.Models.Generated;
using Mapster;
using System;
using System.Linq;

namespace FinanceManagement.Infrastructure.Mappers
{
    public static class CommonMapper
    {
        public static SubscriptionDto Map(this Subscription model)
        {
            var dto = model.Adapt<SubscriptionDto>();

            dto.UserId = model.Account.UserId.Value;
            dto.IsActive = !model.Account.IsDeleted;

            var nextBilling = DateTime.Now.AddMonths(1);
            dto.NextBilling = model.Account.IsDeleted ? null : new DateTime(nextBilling.Year, nextBilling.Month, model.DateFrom.Day);

            return dto;
        }

        public static SubscriptionDto Map(this Subscription model, int userId)
        {
            var dto = model.Adapt<SubscriptionDto>();

            dto.UserId = userId;
            dto.IsActive = true;

            var nextBilling = DateTime.Now.AddMonths(1);
            dto.NextBilling = new DateTime(nextBilling.Year, nextBilling.Month, model.DateFrom.Day);

            return dto;
        }

        public static GroupDto Map(this Group model)
        {
            var dto = model.Adapt<GroupDto>();

            dto.Roles = model.UserGroupRoles.Select(r => r.Adapt<UserGroupRolesDto>()).ToList();
            dto.Currency = Enum.Parse<CurrencyEnum>(model.Account.Currency);
            dto.Amount = model.Account.Amount;

            return dto;
        }
    }
}
