using System.ComponentModel.DataAnnotations;

namespace Data.DataTransfer
{
    public class CreateUserRequest
    {   
        [Required(ErrorMessage = "Valid e-mail address is required")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}