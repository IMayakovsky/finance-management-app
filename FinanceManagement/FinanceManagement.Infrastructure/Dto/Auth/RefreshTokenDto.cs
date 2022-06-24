using System;

namespace FinanceManagement.Infrastructure.Dto.Auth
{
    public class RefreshTokenDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AccessTokenId { get; set; }
        public DateTime Expired { get; set; }
    }
}
