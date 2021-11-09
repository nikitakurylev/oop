using System.Collections.Generic;

namespace Backups.Entities
{
    public class BackupJob
    {
        private readonly IStorage _storage;
        private readonly HashSet<JobObject> _jobObjects;
        private readonly List<RestorePoint> _restorePoints;

        public BackupJob(IStorage storage)
        {
            _jobObjects = new HashSet<JobObject>();
            _restorePoints = new List<RestorePoint>();
            _storage = storage;
        }

        public bool AddJobObject(JobObject jobObject)
        {
            return _jobObjects.Add(jobObject);
        }

        public bool RemoveJobObject(JobObject jobObject)
        {
            return _jobObjects.Remove(jobObject);
        }

        public void CreateRestorePoint()
        {
            _restorePoints.Add(new RestorePoint(_storage, _jobObjects, _restorePoints.Count));
        }
    }
}