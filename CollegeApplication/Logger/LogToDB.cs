namespace CollegeApplication.Logger
{
    public class LogToDB : IMyLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Log to DB");
        }
    }
}
