using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess
{
    public class Chat
    {
        public int Id { get; set; }
        public string? UserToId { get; set; }
        [ForeignKey("UserToId")]
        [NotMapped]
        public virtual ApplicationUser? UserTo { get; set; }
        public string? UserFromId { get; set; }
        [ForeignKey("UserFromId")]
        [NotMapped]
        public virtual ApplicationUser? UserFrom { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? RoomName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
