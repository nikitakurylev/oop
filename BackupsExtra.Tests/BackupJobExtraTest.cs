using System;
using System.Linq;
using Backups.Entities;
using BackupsExtra.Entities;
using NUnit.Framework;

namespace BackupsExtra.Tests
{
    public class BackupJobExtraTest
    {
        [Test]
        public void DeleteCleanerTest()
        {
            var memoryRepository = new MemoryRepositoryExtra();
            var splitStorage = new SplitStorageExtra(memoryRepository);
            var deleteCleaner = new DeleteCleaner(r => false);
            var backupJob = new BackupJobExtra(splitStorage, deleteCleaner);
            var jobObjectA = new JobObject("FileA");
            var jobObjectB = new JobObject("FileB");
            var jobObjectC = new JobObject("FileC");
            backupJob.AddJobObject(jobObjectA);
            backupJob.AddJobObject(jobObjectB);
            backupJob.CreateRestorePoint(new DateTime(2021, 11, 27));
            backupJob.RemoveJobObject(jobObjectA);
            backupJob.CreateRestorePoint(new DateTime(2021, 11, 28));
            Assert.AreEqual(5, memoryRepository.GetFileNames().Count());
            Assert.AreEqual(2,splitStorage.ReadJobObjects(0).Count());
            Assert.AreEqual(1,splitStorage.ReadJobObjects(1).Count());
            backupJob.AddJobObject(jobObjectA);
            backupJob.AddJobObject(jobObjectC);
            deleteCleaner.DeleteCondition =r => r.Time < new DateTime(2021, 11, 28);
            backupJob.CreateRestorePoint(new DateTime(2021, 11, 29));
            Assert.AreEqual(6, memoryRepository.GetFileNames().Count());
            Assert.AreEqual(3,splitStorage.ReadJobObjects(2).Count());
            Assert.AreEqual(1,splitStorage.ReadJobObjects(1).Count());
        }
        
        [Test]
        public void MergeCleanerTest()
        {
            var memoryRepository = new MemoryRepositoryExtra();
            var splitStorage = new SplitStorageExtra(memoryRepository);
            var deleteCleaner = new MergeCleaner(r => false);
            var backupJob = new BackupJobExtra(splitStorage, deleteCleaner);
            var jobObjectA = new JobObject("FileA");
            var jobObjectB = new JobObject("FileB");
            var jobObjectC = new JobObject("FileC");
            backupJob.AddJobObject(jobObjectA);
            backupJob.AddJobObject(jobObjectB);
            backupJob.CreateRestorePoint(new DateTime(2021, 11, 27));
            backupJob.RemoveJobObject(jobObjectA);
            backupJob.CreateRestorePoint(new DateTime(2021, 11, 28));
            Assert.AreEqual(5, memoryRepository.GetFileNames().Count());
            Assert.AreEqual(2,splitStorage.ReadJobObjects(0).Count());
            Assert.AreEqual(1,splitStorage.ReadJobObjects(1).Count());
            backupJob.AddJobObject(jobObjectA);
            backupJob.AddJobObject(jobObjectC);
            deleteCleaner.DeleteCondition =r => deleteCleaner.Count > 2;
            backupJob.CreateRestorePoint(new DateTime(2021, 11, 29));
            Assert.AreEqual(7, memoryRepository.GetFileNames().Count());
            Assert.AreEqual(3,splitStorage.ReadJobObjects(2).Count());
            Assert.AreEqual(2,splitStorage.ReadJobObjects(1).Count());
        }
    }
}