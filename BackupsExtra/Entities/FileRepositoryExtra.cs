using System.Collections.Generic;
using System.IO;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class FileRepositoryExtra : IRepositoryExtra
    {
        private FileRepository _repository;
        private string _directory;

        public FileRepositoryExtra(string directory)
        {
            _directory = directory;
            _repository = new FileRepository(_directory);
        }

        public void Write(string fileName, string data)
        {
            _repository.Write(fileName, data);
        }

        public void WriteArchive(string fileName, Dictionary<string, string> files)
        {
            _repository.WriteArchive(fileName, files);
        }

        public string Read(string fileName)
        {
            return _repository.Read(fileName);
        }

        public Dictionary<string, string> ReadArchive(string fileName)
        {
            return _repository.ReadArchive(fileName);
        }

        public IEnumerable<string> GetFileNames()
        {
            return _repository.GetFileNames();
        }

        public void Delete(string fileName)
        {
            File.Delete(_directory + fileName);
        }

        public override string ToString()
        {
            return "File repository " + _directory;
        }
    }
}