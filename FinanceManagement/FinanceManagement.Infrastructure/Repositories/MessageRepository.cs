using Dapper;
using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface IMessageRepository : IRepository
    {
        Task<List<Message>> GetByIds(List<int> messageIds);
        Task<List<Message>> GetCreatedMessagesByTypeAndProcess(int messageTypeId, int limit);
    }

    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public async Task<List<Message>> GetByIds(List<int> messageIds)
        {
            return await Entities.Where(e => messageIds.Contains(e.Id)).ToListAsync();
        }

        public async Task<List<Message>> GetCreatedMessagesByTypeAndProcess(int messageTypeId, int limit)
        {
            DynamicParameters sqlParameter = new DynamicParameters();

            sqlParameter.Add("messages_count", limit);
            sqlParameter.Add("message_type", messageTypeId);

            return (await DbConnection.QueryAsync<Message>("dbo.get_created_messages", sqlParameter, commandType: System.Data.CommandType.StoredProcedure)).ToList();
        }
    }
}
