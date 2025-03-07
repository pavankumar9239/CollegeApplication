namespace CollegeApplication.Models
{
    public class CollegeRepository
    {
        public static List<Student> Students = new List<Student>()
        {
            new Student()
                {
                    Id = 1,
                    Name = "APK",
                    Email = "apk@gmail.com",
                    Address = "HYD, INDIA"
                },
                new Student() {
                    Id = 2,
                    Name = "Poojita",
                    Email = "poojita@gmail.com",
                    Address = "HYD, INDIA"
                }
        };
    }
}
