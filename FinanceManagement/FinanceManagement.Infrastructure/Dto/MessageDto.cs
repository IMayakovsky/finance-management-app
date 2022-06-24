using FinanceManagement.Infrastructure.Dto.Enums;

namespace FinanceManagement.Infrastructure.Dto
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Receiver { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public MessageTypeEnum MessageType { get; set; }
        public MessageStatusEnum MessageStatus { get; set; }
    }
}
