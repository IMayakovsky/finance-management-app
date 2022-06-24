using Dawn;
using FinanceManagement.Core.Exceptions;
using FinanceManagement.Core.Operations;
using FinanceManagement.Infrastructure.Common.Constants;
using FinanceManagement.Infrastructure.Database;
using FinanceManagement.Infrastructure.Dto.Auth;
using FinanceManagement.Infrastructure.Mappers;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Operations.Base;
using FinanceManagement.Infrastructure.Operations.Transients;
using FinanceManagement.Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ClaimTypes = FinanceManagement.Infrastructure.Dto.Auth.ClaimTypes;

namespace FinanceManagement.Infrastructure.Operations.Singletons
{
    public interface IAuthenticationOperation : ISingletonOperation
    {
        /// <summary>
        /// Registers new user
        /// </summary>
        Task Register(RegisterRequest request);

        /// <summary>
        /// Validates password
        /// </summary>
        /// <returns>true if password is valid, false otherwise</returns>
        bool ValidatePassword(string possiblePassword, string realPasswordHash);

        /// <summary>
        /// Returns generated access and refresh tokens
        /// About token types: https://auth0.com/blog/refresh-tokens-what-are-they-and-when-to-use-them/
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        TokenResult GenerateJwtTokens(User user, bool rememberMe);

        /// <summary>
        /// Stores hash user id, which uses for user identification in password restore
        /// </summary>
        /// <param name="userLogin">user login</param>
        /// <returns>hash user id</returns>
        Task<string> StoreUserResetId(string userLogin);

        /// <summary>
        /// Compares {hashId} with stored hash user id
        /// </summary>
        /// <param name="hashId">hash id for comparing</param>
        /// <returns>true if id is equal, false otherwise</returns>
        Task<bool> CompareWithStoredUserResetId(string hashId);

        /// <summary>
        /// Changes User's password
        /// </summary>
        /// <param name="request">request with user hashId and new password</param>
        /// <returns>Response with user IdentityId from Charon</returns>
        Task<RestorePasswordResponse> RestorePassword(RestorePasswordRequest request);
    }

    public class AuthenticationOperation : BaseInfrastructureOperation, IAuthenticationOperation
    {
        private readonly TokenValidationParameters tokenValidationParameters;
        private readonly IDataAccess dataAccess;
        private readonly Lazy<IUserOperation> userOperation;

        public AuthenticationOperation(TokenValidationParameters tokenValidationParameters, 
            Lazy<IUserOperation> userOperation, IDataAccess dataAccess)
        {
            this.tokenValidationParameters = tokenValidationParameters;
            this.userOperation = userOperation;
            this.dataAccess = dataAccess;
        }

        public async Task Register(RegisterRequest request)
        {
            using (var md5 = MD5.Create())
            {
                request.Password = GetMd5Hash(md5, request.Password);
            }

            try
            {
                await dataAccess.Repository<IUserRepository>().InsertAndSaveAsync(request.ToUser());
            }
            catch(Exception e)
            {
                throw new BaseException("User cannot be saved to database", e);
            }
        }

        public bool ValidatePassword(string possiblePassword, string realPasswordHash)
        {
            using (var md5 = MD5.Create())
            {
                string possibleHash = GetMd5Hash(md5, possiblePassword);

                StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                return comparer.Compare(possibleHash, realPasswordHash) == 0;
            }
        }

        public TokenResult GenerateJwtTokens(User user, bool rememberMe)
        {
            Guard.Argument(user, nameof(user)).NotNull();

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.UserId, user.Id.ToString()),
                new Claim(ClaimTypes.IsAdmin, user.IsAdmin.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),   
                Issuer = tokenValidationParameters.ValidIssuer,
                Audience = tokenValidationParameters.ValidAudience,
                SigningCredentials = new SigningCredentials(tokenValidationParameters.IssuerSigningKey, SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshTokenDto()
            {
                UserId = user.Id,
                AccessTokenId = token.Id,
                Expired = DateTime.UtcNow.AddMonths(ExpiryConstants.RefreshTokenMonths)
            };

            return new TokenResult
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken,
                Success = true
            };
        }

        public async Task<string> StoreUserResetId(string email)
        {
            Guard.Argument(email, nameof(email)).NotNull();

            var user = await dataAccess.Repository<IUserRepository>().GetByEmail(email);

            if (user == null)
            {
                return null;
            }

            string id = Guid.NewGuid() + email + DateTime.UtcNow.Ticks;

            using (var md5 = MD5.Create())
            {
                id = GetMd5Hash(md5, id);
            }

            var model = new RestorePassword
            {
                UserId = user.Id,
                UserResetId = id,
                Expired = DateTime.UtcNow.AddMonths(ExpiryConstants.UserResetIdMinutes)
            };

            await dataAccess.Repository<IRestorePasswordRepository>().InsertAndSaveAsync(model);

            return id;
        }

        public async Task<bool> CompareWithStoredUserResetId(string userResetId)
        {
            var model = await dataAccess.Repository<IRestorePasswordRepository>().GetByUserResetId(userResetId);
            
            return model != null && model.Expired > DateTime.UtcNow;
        }

        public async Task<RestorePasswordResponse> RestorePassword(RestorePasswordRequest request)
        {
            Guard.Argument(request, nameof(request)).NotNull();

            var restorePasswordModel = await dataAccess.Repository<IRestorePasswordRepository>().GetByUserResetId(request.HashId);

            if (restorePasswordModel == null || restorePasswordModel.Expired <= DateTime.UtcNow)
            {
                return new RestorePasswordResponse { Success = false };
            }

            var userModel = await dataAccess.Repository<IUserRepository>().GetById(restorePasswordModel.UserId);

            using (var md5 = MD5.Create())
            {
                userModel.Password = GetMd5Hash(md5, request.NewPassword);
            }

            await dataAccess.Repository<IUserRepository>().UpdateAndSaveAsync(userModel);

            return new RestorePasswordResponse { Success = true, UserId = userModel.Id };
        }

        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
