using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface IUserGroupRoleRepository : IRepository
    {
        Task<ILookup<int, UserGroupRole>> GetByUserId(int userId);
        Task<List<UserGroupRole>> GetByGroupId(int groupId);
    }

    public class UserGroupRoleRepository : BaseRepository<UserGroupRole>, IUserGroupRoleRepository
    {
        public async Task<ILookup<int, UserGroupRole>> GetByUserId(int userId)
        {
            return (await Entities.Where(e => e.UserId == userId && !e.Group.Account.IsDeleted).ToListAsync()).ToLookup(e => e.GroupId, e => e);
        }

        public async Task<List<UserGroupRole>> GetByGroupId(int groupId)
        {
            return await Entities.Where(e => e.GroupId == groupId).ToListAsync();
        }
    }
}
