using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess
{
    public class ChatMessage
    {
        public long Id { get; set; }
        public string? FromUserId { get; set; }
        public string? ToUserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? RoomName { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ApplicationUser FromUser { get; set; }
        public virtual ApplicationUser ToUser { get; set; }
    }
}
