namespace CollegeApplication.Logger
{
    public class LogToFile : IMyLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Log to file");
        }
    }
}
