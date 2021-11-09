using System.Collections.Generic;

namespace Backups.Entities
{
    public interface IStorage
    {
        void WriteJobObjects(HashSet<JobObject> jobObjects, int number);
        HashSet<JobObject> ReadJobObjects(int number);
    }
}