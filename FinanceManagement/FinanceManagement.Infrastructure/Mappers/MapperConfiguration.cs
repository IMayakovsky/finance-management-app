using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Dto.Groups;
using FinanceManagement.Infrastructure.Models.Generated;
using Mapster;
using System;

namespace FinanceManagement.Infrastructure.Mappers
{
    public static class MapperConfiguration
    {
        public static void Configure()
        {
            Configure(TypeAdapterConfig.GlobalSettings);
        }

        public static void Configure(TypeAdapterConfig config)
        {
            config.NewConfig<NotificationDto, Notification>()
                .Map(s => s.NotificationTypeId, m => (int) m.NotificationType);
            config.NewConfig<Notification, NotificationDto>()
                .Map(s => s.NotificationType, m => (NotificationTypeEnum) m.NotificationTypeId);

            config.NewConfig<UserGroupRolesDto, UserGroupRole>()
                .Map(s => s.GroupRoleId, m => (int) m.Role);
            config.NewConfig<UserGroupRole, UserGroupRolesDto>()
                .Map(s => s.Role, m => (GroupRoleEnum) m.GroupRoleId);

            config.NewConfig<AccountDto, Account>()
                .Map(s => s.Currency, m => m.Currency.ToString());
            config.NewConfig<Account, AccountDto>()
                .Map(s => s.Currency, m => Enum.Parse<CurrencyEnum>(m.Currency));

            config.NewConfig<GoalDto, Goal>()
                .Map(s => s.Currency, m => m.Currency.ToString());
            config.NewConfig<Goal, GoalDto>()
                .Map(s => s.Currency, m => Enum.Parse<CurrencyEnum>(m.Currency));

            config.NewConfig<DebtDto, Debt>()
                .Map(s => s.Currency, m => m.Currency.ToString());
            config.NewConfig<Debt, DebtDto>()
                .Map(s => s.Currency, m => Enum.Parse<CurrencyEnum>(m.Currency));

            config.NewConfig<MessageDto, Message>()
                .Map(s => s.MessageStatusId, m => (int)m.MessageStatus)
                .Map(s => s.MessageTypeId, m => (int)m.MessageType)
                .Ignore(s => s.MessageType)
                .Ignore(s => s.MessageStatus);
            config.NewConfig<Message, MessageDto>()
                .Map(s => s.MessageStatus, m => (MessageStatusEnum) m.MessageStatusId)
                .Map(s => s.MessageType, m => (MessageTypeEnum) m.MessageTypeId);
        }
    }
}
