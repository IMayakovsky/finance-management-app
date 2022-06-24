using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface IGroupRepository : IRepository
    {
        Task<List<Group>> GetByUserId(int userId);
        Task<Group> GetById(int groupId);
        Task<List<Group>> GetByIdsWithRolesAndAcount(List<int> ids);
        Task<List<int>> GetGroupIdsPage(int count, int skip);
    }

    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public async Task<List<Group>> GetByUserId(int userId)
        {
            return await Entities.Where(e => !e.UserGroupRoles.Any(r => r.UserId == userId)).Include(e => e.UserGroupRoles.Where(r => r.UserId == userId)).ToListAsync();
        }

        public async Task<Group> GetById(int groupId)
        {
            return await Entities.FirstOrDefaultAsync(g => g.Id == groupId && !g.Account.IsDeleted);
        }

        public async Task<List<Group>> GetByIdsWithRolesAndAcount(List<int> ids)
        {
            return await Entities.Where(e => ids.Contains(e.Id) && !e.Account.IsDeleted).Include(e => e.UserGroupRoles).Include(e => e.Account).ToListAsync();
        }

        public async Task<List<int>> GetGroupIdsPage(int count, int skip)
        {
            return await this.Entities
                .OrderBy(s => s.Id)
                .Select(s => s.Id)
                .Skip(skip)
                .Take(count)
                .ToListAsync();
        }
    }
}
