using System;
using System.Collections.Generic;

namespace Backups.Entities
{
    public class MemoryRepository : IRepository
    {
        private readonly Dictionary<string, string> _files;

        public MemoryRepository()
        {
            _files = new Dictionary<string, string>();
        }

        public void Write(string fileName, string data)
        {
            _files[fileName] = data;
        }

        public void WriteArchive(string archiveName, Dictionary<string, string> files)
        {
            string archiveData = string.Empty;
            foreach ((string fileName, string data) in files)
                archiveData += fileName + ":" + data + ";";
            _files[archiveName] = archiveData;
        }

        public string Read(string fileName)
        {
            return _files[fileName];
        }

        public Dictionary<string, string> ReadArchive(string archiveName)
        {
            var archiveFiles = new Dictionary<string, string>();
            foreach (string archiveEntry in Read(archiveName).Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                string[] nameDataPair = archiveEntry.Split(':');
                archiveFiles[nameDataPair[0]] = nameDataPair[1];
            }

            return archiveFiles;
        }

        public IEnumerable<string> GetFileNames()
        {
            return _files.Keys;
        }
    }
}