namespace FinanceManagement.Infrastructure.Dto.Auth
{
    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
