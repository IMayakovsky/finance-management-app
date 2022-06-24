using System;
using System.Collections.Generic;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class MessageType
    {
        public MessageType()
        {
            Messages = new HashSet<Message>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
