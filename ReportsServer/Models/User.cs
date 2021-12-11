namespace ReportsServer.Models
{
    public class User : IModel
    {
        public User(string uid, string password, string boss)
        {
            Uid = uid;
            Password = password;
            Boss = boss;
        }
        public string Uid { get; set; }
        public string Password { get; set; }
        public string Boss { get; set; }
    }
}