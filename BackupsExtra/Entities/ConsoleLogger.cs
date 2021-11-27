using System;

namespace BackupsExtra.Entities
{
    public class ConsoleLogger : ILogger
    {
        public bool TimeStamp { get; set; } = false;
        public void Log(object obj)
        {
            if (TimeStamp)
                Console.WriteLine(DateTime.Now + " " + obj);
            else
                Console.WriteLine(obj);
        }
    }
}