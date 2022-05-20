using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ChatMessageDTO
    {
        public long Id { get; set; }
        public string? UserFromId { get; set; }
        public string? UserToId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
