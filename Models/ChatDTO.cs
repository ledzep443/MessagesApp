using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public string? UserFromId { get; set; }
        public UserDTO? UserFrom { get; set; }
        public string? UserToId { get; set; }
        public UserDTO? UserTo { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
