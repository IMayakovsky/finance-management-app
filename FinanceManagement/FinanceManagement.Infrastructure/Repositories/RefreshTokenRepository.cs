using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface IRefreshTokenRepository : IRepository
    {
        Task<RefreshToken> GetToken(int userId, string accessTokenId);
        Task<List<RefreshToken>> GetByUserId(int userId);
    }

    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public async Task<RefreshToken> GetToken(int userId, string accessTokenId)
        {
            return await Entities.FirstOrDefaultAsync(e => e.UserId == userId && e.AccessTokenId == accessTokenId);
        }

        public async Task<List<RefreshToken>> GetByUserId(int userId)
        {
            return await Entities.ToListAsync();
        }
    }
}
