using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface IRestorePasswordRepository : IRepository
    {
        Task<RestorePassword> GetByUserResetId(string userResetId);
        Task<List<RestorePassword>> GetByUserId(int userId);
    }

    public class RestorePasswordRepository : BaseRepository<RestorePassword>, IRestorePasswordRepository
    {
        public async Task<RestorePassword> GetByUserResetId(string userResetId)
        {
            return await Entities.FirstOrDefaultAsync(e => e.UserResetId == userResetId);
        }

        public async Task<List<RestorePassword>> GetByUserId(int userId)
        {
            return await Entities.Where(e => e.UserId == userId).ToListAsync();
        }
    }
}
