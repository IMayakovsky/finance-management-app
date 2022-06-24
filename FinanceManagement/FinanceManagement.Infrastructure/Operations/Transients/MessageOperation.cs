using FinanceManagement.Core.Operations;
using FinanceManagement.Infrastructure.Database;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Operations.Base;
using FinanceManagement.Infrastructure.Repositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Operations.Transients
{
    public interface IMessageOperation : ITransientOperation
    {
        Task<List<MessageDto>> GetCreatedEmailMessages(int limit);
        Task ProcessMessages(List<int> messageIds);
        Task CreateMessage(MessageDto message);
        Task UpdateMessages(List<MessageDto> messages);
    }

    public class MessageOperation : BaseInfrastructureOperation, IMessageOperation
    {
        private readonly IDataAccess dataAccess;

        public MessageOperation(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public Task UpdateMessages(Dictionary<int, MessageStatusEnum> statuses)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MessageDto>> GetCreatedEmailMessages(int limit)
        {
            var models = await dataAccess.Repository<IMessageRepository>().GetCreatedMessagesByTypeAndProcess((int) MessageTypeEnum.Email, limit);

            return models.Select(m => m.Adapt<MessageDto>()).ToList();
        }

        public async Task ProcessMessages(List<int> messageIds)
        {
            var models = await dataAccess.Repository<IMessageRepository>().GetByIds(messageIds);

            foreach (var model in models)
            {
                model.MessageStatusId = (int) MessageStatusEnum.InProgress;
            }

            await dataAccess.Repository<IMessageRepository>().UpdateRangeAndSaveAsync(models);
        }

        public async Task CreateMessage(MessageDto message)
        {
            message.MessageStatus = MessageStatusEnum.Created;
            var model = message.Adapt<Message>();
            await dataAccess.Repository<IMessageRepository>().InsertAndSaveAsync(model);
        }

        public async Task UpdateMessages(List<MessageDto> messages)
        {
            await dataAccess.Repository<IMessageRepository>().UpdateRangeAndSaveAsync(messages.Select(m => m.Adapt<Message>()).ToList());
        }
    }
}
