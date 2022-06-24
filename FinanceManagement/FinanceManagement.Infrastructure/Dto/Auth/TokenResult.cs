namespace FinanceManagement.Infrastructure.Dto.Auth
{
    public class TokenResult
    {
        /// <summary>
        /// Access tokens carry the necessary information to access a resource directly.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Refresh tokens carry the information necessary to get a new access token
        /// </summary>
        public RefreshTokenDto RefreshToken { get; set; }

        public bool Success { get; set; }

        public string Error { get; set; }
    }
}
