using System.ComponentModel.DataAnnotations;

namespace BlogApi.Requests
{
    public class NewUserModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
