using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "Employee";
        public string HireDate { get; set; }
    }
}