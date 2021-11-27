using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface IStorageExtra : IStorage
    {
        public List<int> GetRestorePointNumbers();
        void DeleteJob(int number);
    }
}