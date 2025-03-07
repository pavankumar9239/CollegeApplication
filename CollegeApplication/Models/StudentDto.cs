using System.ComponentModel.DataAnnotations;

namespace CollegeApplication.Models
{
    public class StudentDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Student name is required.")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
