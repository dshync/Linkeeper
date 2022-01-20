using System.ComponentModel.DataAnnotations;

namespace Linkeeper.DTOs.ApiIdentity
{
    //used only for API
    public class UserLoginRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
