namespace Backups.Entities
{
    public class JobObject
    {
        public JobObject(string name, string data)
        {
            Name = name;
            Data = data;
        }

        public string Name { get; }
        public string Data { get; }
    }
}