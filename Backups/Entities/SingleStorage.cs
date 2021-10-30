using System.Collections.Generic;

namespace Backups.Entities
{
    public class SingleStorage : IStorage
    {
        private readonly IRepository _repository;

        public SingleStorage(IRepository repository)
        {
            _repository = repository;
        }

        public void WriteJobObjects(HashSet<JobObject> jobObjects, int number)
        {
            var files = new Dictionary<string, string>();
            foreach (JobObject jobObject in jobObjects)
                files[jobObject.Name] = jobObject.Name;
            _repository.WriteArchive(number.ToString(), files);
        }

        public HashSet<JobObject> ReadJobObjects(int number)
        {
            var jobObjects = new HashSet<JobObject>();
            foreach ((string fileName, string data) in _repository.ReadArchive(number.ToString()))
                jobObjects.Add(new JobObject(fileName));

            return jobObjects;
        }
    }
}