using System;
using System.IO;

namespace BackupsExtra.Entities
{
    public class FileLogger : ILogger
    {
        private string _file;

        public FileLogger(string file)
        {
            this._file = file;
        }

        public bool TimeStamp { get; set; } = false;
        public void Log(object obj)
        {
            if (TimeStamp)
                File.AppendAllText(_file, DateTime.Now + " " + obj + "\n");
            else
                File.AppendAllText(_file, obj + "\n");
        }
    }
}