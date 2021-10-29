using System.Linq;
using Backups.Entities;
using NUnit.Framework;

namespace Backups.Tests
{
    public class BackupJobTest
    {
        [Test]
        public void MemorySplitTest()
        {
            var memoryRepository = new MemoryRepository();
            var splitStorage = new SplitStorage(memoryRepository);
            var backupJob = new BackupJob(splitStorage);
            var jobObjectA = new JobObject("FileA", "DataA");
            var jobObjectB = new JobObject("FileB", "DataB");
            backupJob.AddJobObject(jobObjectA);
            backupJob.AddJobObject(jobObjectB);
            backupJob.CreateRestorePoint();
            backupJob.RemoveJobObject(jobObjectA);
            backupJob.CreateRestorePoint();
            Assert.AreEqual(3, memoryRepository.GetFileNames().Count());
            Assert.AreEqual(2,splitStorage.ReadJobObjects(0).Count());
            Assert.AreEqual(1,splitStorage.ReadJobObjects(1).Count());
        }
        
        [Test]
        public void MemorySingleTest()
        {
            var memoryRepository = new MemoryRepository();
            var singleStorage = new SingleStorage(memoryRepository);
            var backupJob = new BackupJob(singleStorage);
            var jobObjectA = new JobObject("FileA", "DataA");
            var jobObjectB = new JobObject("FileB", "DataB");
            backupJob.AddJobObject(jobObjectA);
            backupJob.AddJobObject(jobObjectB);
            backupJob.CreateRestorePoint();
            backupJob.RemoveJobObject(jobObjectA);
            backupJob.CreateRestorePoint();
            Assert.AreEqual(2, memoryRepository.GetFileNames().Count());
            Assert.AreEqual(2,singleStorage.ReadJobObjects(0).Count());
            Assert.AreEqual(1,singleStorage.ReadJobObjects(1).Count());
        }
    }
}