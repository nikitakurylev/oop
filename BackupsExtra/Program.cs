using System;
using System.IO;
using Backups.Entities;
using BackupsExtra.Entities;

namespace BackupsExtra
{
    internal class Program
    {
        private static void Main()
        {
            if (Directory.Exists("../tests/"))
                Directory.Delete("../tests/", true);
            Directory.CreateDirectory("../tests/");
            Directory.CreateDirectory("../tests/test1/");
            FileSplitTest();
            FileSplitRestoreTest();
        }

        private static void FileSplitTest()
        {
            var fileRepository = new FileRepositoryExtra("../tests/test1/");
            var splitStorage = new SplitStorageExtra(fileRepository);
            var backupJob = new BackupJobExtra(splitStorage, new DeleteCleaner(r => r.Time - DateTime.Now > TimeSpan.FromMinutes(10)));
            var jobObjectA = new JobObject("FileA");
            var jobObjectB = new JobObject("FileB");
            var logger = new FileLogger("../tests/log.txt");
            logger.TimeStamp = true;
            logger.Log(backupJob);
            backupJob.AddJobObject(jobObjectA);
            backupJob.AddJobObject(jobObjectB);
            backupJob.CreateRestorePoint(DateTime.Now);
            logger.Log(backupJob);
            backupJob.RemoveJobObject(jobObjectA);
            backupJob.CreateRestorePoint(DateTime.Now);
            logger.Log(backupJob);
        }

        private static void FileSplitRestoreTest()
        {
            var logger = new ConsoleLogger();
            var fileRepository = new FileRepositoryExtra("../tests/test1/");
            var splitStorage = new SplitStorageExtra(fileRepository);
            var backupJob = new BackupJobExtra(splitStorage, new DeleteCleaner(r => r.Time - DateTime.Now > TimeSpan.FromMinutes(10)));
            logger.Log(backupJob);
        }
    }
}
