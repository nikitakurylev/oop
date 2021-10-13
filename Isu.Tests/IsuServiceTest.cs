using Isu.Entities;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;
        private Group _m3205;
        private Group _m3306;
        private Student _nikita;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService(2);
            _m3205 = _isuService.AddGroup("M3205");
            _nikita = _isuService.AddStudent(_m3205, "Nikita Kurylev");
            _m3306 = _isuService.AddGroup("M3306");
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddStudent(_m3205, "Nikita Kurylev");
            });
            Assert.Catch<IsuException>(() =>
            {
                _m3205.AddStudent(_nikita, 0);
            });
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddStudent(_m3205, "Maxim Rakov");
                _isuService.AddStudent(_m3205, "Nikita Romanov");
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("M3Z05");
            });
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("NS205");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.ChangeStudentGroup(_nikita, _m3306);
            });
        }
    }
}