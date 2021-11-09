using System;
using System.IO;
using Backups.Entities;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            if (Directory.Exists("../tests/"))
                Directory.Delete("../tests/", true);
            Directory.CreateDirectory("../tests/");
            Directory.CreateDirectory("../tests/test1/");
            Directory.CreateDirectory("../tests/test2/");
            FileSplitTest();
            FileSingleTest();
        }

        private static void FileSplitTest()
        {
            var fileRepository = new FileRepository("../tests/test1/");
            var splitStorage = new SplitStorage(fileRepository);
            var backupJob = new BackupJob(splitStorage);
            var jobObjectA = new JobObject("FileA");
            var jobObjectB = new JobObject("FileB");
            backupJob.AddJobObject(jobObjectA);
            backupJob.AddJobObject(jobObjectB);
            backupJob.CreateRestorePoint();
            backupJob.RemoveJobObject(jobObjectA);
            backupJob.CreateRestorePoint();
        }

        private static void FileSingleTest()
        {
            var memoryRepository = new FileRepository("../tests/test2/");
            var singleStorage = new SingleStorage(memoryRepository);
            var backupJob = new BackupJob(singleStorage);
            var jobObjectA = new JobObject("FileA.txt");
            var jobObjectB = new JobObject("FileB.txt");
            backupJob.AddJobObject(jobObjectA);
            backupJob.AddJobObject(jobObjectB);
            backupJob.CreateRestorePoint();
            backupJob.RemoveJobObject(jobObjectA);
            backupJob.CreateRestorePoint();
        }
    }
}
