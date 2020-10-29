using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForLoginDTO
    {
        [Required]
        public string UserName { get; set; } 

        [Required]
        [StringLength(8,MinimumLength=6,ErrorMessage="Enter password between 6 and 8 characters.")]
        public string Password { get; set; }
    }
}