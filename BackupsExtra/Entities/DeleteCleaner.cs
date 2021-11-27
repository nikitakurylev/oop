using System;
using System.Collections.Generic;
using System.Linq;

namespace BackupsExtra.Entities
{
    public class DeleteCleaner : IRestorePointCleaner
    {
        public DeleteCleaner(Predicate<RestorePointExtra> deleteCondition)
        {
            DeleteCondition = deleteCondition;
            Count = 0;
        }

        public Predicate<RestorePointExtra> DeleteCondition { get; set; }
        public int Count { get; private set; }
        public void Clean(List<RestorePointExtra> restorePoints)
        {
            Count = restorePoints.Count;
            foreach (RestorePointExtra restorePoint in restorePoints.FindAll(DeleteCondition))
                restorePoint.Delete();

            restorePoints.RemoveAll(DeleteCondition);
        }
    }
}