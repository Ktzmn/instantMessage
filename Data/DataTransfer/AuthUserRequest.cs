using System.ComponentModel.DataAnnotations;

namespace Data.DataTransfer
{
    public class AuthUserRequest
    {   
        [Required]
        public string Username { get; set; } 

        [Required]   
        public string Password { get; set; }    
    }
}