using System;
using System.Collections.Generic;

namespace ReportsServer.Models
{
    public class Report : IModel
    {
        public string Uid { get; set; }
        public string Creator { get; set; }
        public bool State { get; set; }
        public DateTime CreationDate { get; set; }
        public List<string> Tasks { get; set; }
        public List<string> Reports { get; set; }
    }
}