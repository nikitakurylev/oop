using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class RestorePointExtra
    {
        private RestorePoint _restorePoint;
        private IStorageExtra _storage;

        public RestorePointExtra(IStorageExtra storage, HashSet<JobObject> jobObjects, int number, DateTime creationTime)
        {
            _storage = storage;
            Number = number;
            _restorePoint = new RestorePoint(storage, jobObjects, number);
            Time = creationTime;
        }

        public int Number { get; }
        public DateTime Time { get; }

        public HashSet<JobObject> GetJobObjects()
        {
            return _storage.ReadJobObjects(Number);
        }

        public void Delete()
        {
            _storage.DeleteJob(Number);
        }

        public void Merge(RestorePointExtra oldRestorePoint)
        {
            HashSet<JobObject> oldJobObjects = oldRestorePoint.GetJobObjects();
            HashSet<JobObject> jobObjects = GetJobObjects();
            foreach (JobObject jobObject in oldJobObjects.Where(oldJobObject => jobObjects.All(jobObject => jobObject.Name != oldJobObject.Name)))
                jobObjects.Add(jobObject);
            _restorePoint = new RestorePoint(_storage, jobObjects, Number);
            oldRestorePoint.Delete();
        }

        public override string ToString()
        {
            return "Restore point #" + Number + " " + Time;
        }
    }
}