using System;
using System.Collections.Generic;

namespace Backups.Entities
{
    public class RestorePoint
    {
        public RestorePoint(IStorage storage, HashSet<JobObject> jobObjects, int number)
        {
            Time = DateTime.Now;
            storage.WriteJobObjects(jobObjects, number);
        }

        private DateTime Time { get; }
    }
}