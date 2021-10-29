using System.Collections.Generic;

namespace Backups.Entities
{
    public interface IRepository
    {
        void Write(string fileName, string data);
        void WriteArchive(string fileName, Dictionary<string, string> files);
        string Read(string fileName);
        Dictionary<string, string> ReadArchive(string fileName);
        IEnumerable<string> GetFileNames();
    }
}