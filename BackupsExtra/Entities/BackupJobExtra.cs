using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class BackupJobExtra
    {
        private readonly IStorageExtra _storage;
        private readonly HashSet<JobObject> _jobObjects;
        private readonly List<RestorePointExtra> _restorePoints;
        private IRestorePointCleaner _restorePointCleaner;

        public BackupJobExtra(IStorageExtra storage, IRestorePointCleaner restorePointCleaner)
        {
            _jobObjects = new HashSet<JobObject>();
            _storage = storage;
            _restorePointCleaner = restorePointCleaner;
            _restorePoints = new List<RestorePointExtra>();
            List<int> restorePointNumbers = _storage.GetRestorePointNumbers();
            restorePointNumbers.Sort();
            foreach (int number in restorePointNumbers)
            {
                _restorePoints.Add(new RestorePointExtra(_storage, _storage.ReadJobObjects(number), number, DateTime.Now));
            }

            if (_restorePoints.Any())
                _jobObjects = _restorePoints.Last().GetJobObjects();
        }

        public bool AddJobObject(JobObject jobObject)
        {
            return _jobObjects.Add(jobObject);
        }

        public bool RemoveJobObject(JobObject jobObject)
        {
            return _jobObjects.Remove(jobObject);
        }

        public void CreateRestorePoint(DateTime creationTime)
        {
            _restorePoints.Add(new RestorePointExtra(_storage, _jobObjects, _restorePoints.Count, creationTime));
            _restorePointCleaner.Clean(_restorePoints);
        }

        public override string ToString()
        {
            return "Backup Job: \n " + _storage + "\n " + _jobObjects.Count + " job object(s)\n " + _restorePoints.Count + " restore point(s)\n Cleaner type: " +
                   _restorePointCleaner;
        }
    }
}