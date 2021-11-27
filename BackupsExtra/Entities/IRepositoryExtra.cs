using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface IRepositoryExtra : IRepository
    {
        void Delete(string fileName);
    }
}