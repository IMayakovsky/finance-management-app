using Dawn;
using FinanceManagement.Caching;
using FinanceManagement.Caching.Dependencies;
using FinanceManagement.Core.Operations;
using FinanceManagement.Infrastructure.Database;
using FinanceManagement.Infrastructure.Dto.Auth;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Operations.Base;
using FinanceManagement.Infrastructure.Operations.Singletons;
using FinanceManagement.Infrastructure.Repositories;
using Mapster;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Operations.Transients
{
    public interface ITokenOperation : ITransientOperation
    {
        Task<RefreshTokenDto> GetTokenCached(int userId, string accessTokenId);
        Task AddToken(RefreshTokenDto token);
        Task RemoveToken(int userId, string accessTokenId);
        Task RemoveTokensByUserId(int userId);
    }

    public class TokenOperation : BaseInfrastructureOperation, ITokenOperation
    {
        private readonly IDataAccess dataAccess;
        private readonly ICacheInvalidationOperation cacheInvalidationOperation;

        public TokenOperation(IDataAccess dataAccess, ICacheInvalidationOperation cacheInvalidationOperation)
        {
            this.dataAccess = dataAccess;
            this.cacheInvalidationOperation = cacheInvalidationOperation;
        }

        public async Task<RefreshTokenDto> GetTokenCached(int userId, string accessTokenId)
        {
            return await this.ExecuteMemoryCachedAsync(async () => await GetToken(userId, accessTokenId), Cache.GetKey(typeof(RefreshTokenDto), userId, accessTokenId));
        }

        public async Task AddToken(RefreshTokenDto token)
        {
            Guard.Argument(token, nameof(token)).NotNull();

            await dataAccess.Repository<IRefreshTokenRepository>().InsertAndSaveAsync(token.Adapt<RefreshToken>());

            CacheToken(token);
        }

        public async Task RemoveToken(int userId, string accessTokenId)
        {
            var token = await dataAccess.Repository<IRefreshTokenRepository>().GetToken(userId, accessTokenId);

            if (token == null)
            {
                return;
            }

            await dataAccess.Repository<IRefreshTokenRepository>().RemoveAndSaveAsync(token);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.Token, userId);
        }

        public async Task RemoveTokensByUserId(int userId)
        {
            var tokens = await dataAccess.Repository<IRefreshTokenRepository>().GetByUserId(userId);
            var restorePasswords = await dataAccess.Repository<IRestorePasswordRepository>().GetByUserId(userId);

            try
            {
                dataAccess.Repository<IRefreshTokenRepository>().RemoveRange(tokens);
                dataAccess.Repository<IRestorePasswordRepository>().RemoveRange(restorePasswords);
            }
            finally
            {
                await dataAccess.SaveDbContext();
            }

            cacheInvalidationOperation.Invalidate(CacheDependencyType.Token, userId);
        }

        private async Task<RefreshTokenDto> GetToken(int userId, string accessTokenId)
        {
            var token = await dataAccess.Repository<IRefreshTokenRepository>().GetToken(userId, accessTokenId);

            return token.Adapt<RefreshTokenDto>();
        }

        private void CacheToken(RefreshTokenDto token)
        {
            Guard.Argument(token, nameof(token)).NotNull();

            Cache.Current.Provider.Set(Cache.GetKey(typeof(RefreshTokenDto), token.UserId, token.AccessTokenId), token);
            Cache.Current.Dependencies.AddDependencies(Cache.GetKey(typeof(RefreshTokenDto), token.UserId, token.AccessTokenId), new CacheDependency(CacheDependencyType.Token, token.UserId));
        }
    }
}
