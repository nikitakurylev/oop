using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups.Entities
{
    public class FileRepository : IRepository
    {
        private readonly string _directory;

        public FileRepository(string directory)
        {
            _directory = directory;
        }

        public void Write(string fileName, string data)
        {
            File.WriteAllText(_directory + fileName, data);
        }

        public void WriteArchive(string archiveName, Dictionary<string, string> files)
        {
            string tmpPath = CreateTempDir();
            foreach ((string fileName, string data) in files)
                File.WriteAllText(tmpPath + fileName, data);
            ZipFile.CreateFromDirectory(tmpPath, _directory + archiveName);
            Directory.Delete(tmpPath, true);
        }

        public string Read(string fileName)
        {
            return File.ReadAllText(_directory + fileName);
        }

        public Dictionary<string, string> ReadArchive(string archiveName)
        {
            string tmpPath = CreateTempDir();
            var archiveFiles = new Dictionary<string, string>();
            ZipFile.ExtractToDirectory(_directory + archiveName, tmpPath);
            foreach (string fileName in Directory.GetFiles(tmpPath))
                archiveFiles[fileName] = File.ReadAllText(tmpPath + fileName);
            Directory.Delete(tmpPath, true);

            return archiveFiles;
        }

        public IEnumerable<string> GetFileNames()
        {
            return Directory.GetFiles(_directory);
        }

        private string CreateTempDir()
        {
            string path = _directory + "tmp/";
            if (Directory.Exists(path))
                Directory.Delete(path);
            Directory.CreateDirectory(path);
            return path;
        }
    }
}