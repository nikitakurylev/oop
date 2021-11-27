using System.Collections.Generic;

namespace BackupsExtra.Entities
{
    public interface IRestorePointCleaner
    {
        public void Clean(List<RestorePointExtra> restorePoints);
    }
}