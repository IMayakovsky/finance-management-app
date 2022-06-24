namespace FinanceManagement.Infrastructure.Dto.Auth
{
    public class RestorePasswordRequest
    {
        /// <summary>
        /// Using for user identification in password restore
        /// Hash Id is sent to user email as endpoint parameter e.g /api/restore/{hashId}
        /// </summary>
        public string HashId { get; set; }

        /// <summary>
        /// New user password
        /// </summary>
        public string NewPassword { get; set; }
    }
}
