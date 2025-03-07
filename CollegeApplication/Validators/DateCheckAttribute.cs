using System.ComponentModel.DataAnnotations;

namespace CollegeApplication.Validators
{
    public class DateCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date  = (DateTime)value;

            if(date < DateTime.Now)
            {
                return new ValidationResult("Admission date should be greater than current date.");
            }

            return ValidationResult.Success;
        }
    }
}
