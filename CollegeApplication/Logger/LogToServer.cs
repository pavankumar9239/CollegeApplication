﻿namespace CollegeApplication.Logger
{
    public class LogToServer : IMyLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Log to server");
        }
    }
}
