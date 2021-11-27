using System.Collections.Generic;
using System.Linq;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class MemoryRepositoryExtra : IRepositoryExtra
    {
        private MemoryRepository _memoryRepository;
        private HashSet<string> _deletedFiles;

        public MemoryRepositoryExtra()
        {
            _memoryRepository = new MemoryRepository();
            _deletedFiles = new HashSet<string>();
        }

        public void Write(string fileName, string data)
        {
            _memoryRepository.Write(fileName, data);
            _deletedFiles.Remove(fileName);
        }

        public void WriteArchive(string fileName, Dictionary<string, string> files)
        {
            _memoryRepository.WriteArchive(fileName, files);
            _deletedFiles.Remove(fileName);
        }

        public string Read(string fileName)
        {
            if (_deletedFiles.Contains(fileName))
                return null;
            return _memoryRepository.Read(fileName);
        }

        public Dictionary<string, string> ReadArchive(string fileName)
        {
            if (_deletedFiles.Contains(fileName))
                return null;
            return _memoryRepository.ReadArchive(fileName);
        }

        public IEnumerable<string> GetFileNames()
        {
            return _memoryRepository.GetFileNames().Except(_deletedFiles);
        }

        public void Delete(string fileName)
        {
            _deletedFiles.Add(fileName);
        }
    }
}