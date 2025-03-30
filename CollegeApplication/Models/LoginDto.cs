using System.ComponentModel.DataAnnotations;

namespace CollegeApplication.Models
{
    public class LoginDto
    {
        [Required]
        public string Policy { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
