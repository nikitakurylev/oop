using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class SplitStorageExtra : IStorageExtra
    {
        private static readonly string ConfigFileName = "RestorePoint";
        private IRepositoryExtra _repositoryExtra;
        private SplitStorage _storage;

        public SplitStorageExtra(IRepositoryExtra repositoryExtra)
        {
            _repositoryExtra = repositoryExtra;
            _storage = new SplitStorage(repositoryExtra);
        }

        public List<int> GetRestorePointNumbers()
        {
            var result = new List<int>();
            IEnumerable<string> filenames = _repositoryExtra.GetFileNames();
            foreach (string filename in filenames.Where(s => s.StartsWith(ConfigFileName)))
                result.Add(int.Parse(filename[ConfigFileName.Length..]));
            return result;
        }

        public void WriteJobObjects(HashSet<JobObject> jobObjects, int number)
        {
            _storage.WriteJobObjects(jobObjects, number);
            string configData = string.Empty;
            foreach (JobObject jobObject in jobObjects)
                configData += jobObject.Name + ",";
            configData = configData[..^1];
            _repositoryExtra.Write(GetPointName(number), configData);
        }

        public HashSet<JobObject> ReadJobObjects(int number)
        {
            if (_repositoryExtra.GetFileNames().Contains(GetPointName(number)))
            {
                string[] storedPoint = _repositoryExtra.Read(GetPointName(number)).Split(',');
                var result = new HashSet<JobObject>();
                foreach (string fileName in storedPoint)
                    result.Add(new JobObject(_repositoryExtra.Read(GetNumberName(fileName, number))));
                return result;
            }
            else
            {
                return _storage.ReadJobObjects(number);
            }
        }

        public void DeleteJob(int number)
        {
            HashSet<JobObject> jobObjects = ReadJobObjects(number);
            foreach (JobObject jobObject in jobObjects)
                _repositoryExtra.Delete(GetNumberName(jobObject.Name, number));
            _repositoryExtra.Delete(GetPointName(number));
        }

        public override string ToString()
        {
            return "Split storage at " + _repositoryExtra;
        }

        private static string GetNumberName(string name, int number)
        {
            return name + "_" + number;
        }

        private static string GetPointName(int number)
        {
            return ConfigFileName + number;
        }
    }
}