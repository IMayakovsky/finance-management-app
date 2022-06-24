using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class Message
    {
        public int Id { get; set; }
        public string Receiver { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int MessageTypeId { get; set; }
        public int MessageStatusId { get; set; }

        public virtual MessageStatus MessageStatus { get; set; }
        public virtual MessageType MessageType { get; set; }
    }
}
