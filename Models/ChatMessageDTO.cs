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
        public string? FromUserId { get; set; }
        public virtual ApplicationUser? FromUser { get; set; }
        public string? ToUserId { get; set; }
        public virtual ApplicationUser? ToUser { get; set; }
        public string RoomName { get; set; } = "public";
        public string Message { get; set; }
        public string CreatedDate { get; set; }
    }
}
