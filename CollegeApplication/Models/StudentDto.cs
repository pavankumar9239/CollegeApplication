using CollegeApplication.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CollegeApplication.Models
{
    public class StudentDto
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required(ErrorMessage = "Student name is required.")]
        //[StringLength(maximumLength: 30, ErrorMessage = "Name should be less than 30 characters.")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Age is required.")]
        //[Range(10, 20, ErrorMessage = "Age should be between 10 and 20")]
        //public int Age { get; set; }
        [EmailAddress(ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        //public string Password { get; set; }
        ////[Compare("Password", ErrorMessage = "Password and Confirm Password should match.")]
        //[Compare(nameof(Password), ErrorMessage = "Password and Confirm Password should match.")]
        //public string ConfirmPassword { get; set; }
        //[DateCheck]
        public DateTime DOB { get; set; }
    }
}
