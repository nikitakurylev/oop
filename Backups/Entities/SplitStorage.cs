using System.Collections.Generic;

namespace Backups.Entities
{
    public class SplitStorage : IStorage
    {
        private readonly IRepository _repository;
        private readonly Dictionary<int, HashSet<string>> _fileNames;
        public SplitStorage(IRepository repository)
        {
            _repository = repository;
            _fileNames = new Dictionary<int, HashSet<string>>();
        }

        public void WriteJobObjects(HashSet<JobObject> jobObjects, int number)
        {
            _fileNames[number] = new HashSet<string>();
            foreach (JobObject jobObject in jobObjects)
            {
                _repository.Write(GetNumberName(jobObject.Name, number), jobObject.Name);
                _fileNames[number].Add(jobObject.Name);
            }
        }

        public HashSet<JobObject> ReadJobObjects(int number)
        {
            var jobObjects = new HashSet<JobObject>();
            foreach (string fileName in _fileNames[number])
            {
                jobObjects.Add(new JobObject(fileName));
            }

            return jobObjects;
        }

        private static string GetNumberName(string name, int number)
        {
            return name + "_" + number;
        }
    }
}