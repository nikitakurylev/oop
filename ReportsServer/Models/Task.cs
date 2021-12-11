using System;

namespace ReportsServer.Models
{
    public class Task : IModel
    {


        public string Uid { get; set; }
        public string Title { get; set; }
        public string Creator { get; set; }
        public TaskState State { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastChangedDate { get; set; }
    }
}