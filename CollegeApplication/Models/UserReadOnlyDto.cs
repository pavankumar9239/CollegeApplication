namespace CollegeApplication.Models
{
    public class UserReadOnlyDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
