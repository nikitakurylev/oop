using System;

namespace ReportsServer.Models
{
    public class Session : IModel
    {
        public Session(string username)
        {
            Username = username;
            Uid = username + new Random().Next().ToString("X");
        }
        public string Uid { get; set; }
        public string Username { get; set; }
    }
}