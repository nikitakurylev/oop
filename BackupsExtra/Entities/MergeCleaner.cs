using System;
using System.Collections.Generic;
using System.Linq;

namespace BackupsExtra.Entities
{
    public class MergeCleaner : IRestorePointCleaner
    {
        public MergeCleaner(Predicate<RestorePointExtra> deleteCondition)
        {
            DeleteCondition = deleteCondition;
            Count = 0;
        }

        public Predicate<RestorePointExtra> DeleteCondition { get; set; }
        public int Count { get; private set; }
        public void Clean(List<RestorePointExtra> restorePoints)
        {
            Count = restorePoints.Count();
            var orderedRestorePoints = restorePoints.OrderBy(r => r.Number).ToList();
            for (int i = 0; i < orderedRestorePoints.Count(); i++)
            {
                if (DeleteCondition.Invoke(orderedRestorePoints[i]))
                {
                    Count--;
                    orderedRestorePoints[i + 1].Merge(orderedRestorePoints[i]);
                }
            }

            restorePoints.RemoveAll(DeleteCondition);
        }
    }
}