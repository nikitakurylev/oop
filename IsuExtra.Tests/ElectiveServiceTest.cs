using System;
using System.Linq;
using Isu.Entities;
using Isu.Services;
using IsuExtra.Entities;
using IsuExtra.Services;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class Tests
    {
        private IsuService _isuService;
        private Group _m3205;
        private Group _t3200;
        private Student _nikita, _ivan;
        private ElectiveService _electiveService;
        private Elective _securityElective;
        private Elective _biologyElective;
        private ElectiveStream _securityStream1;
        private ElectiveStream _biologyStream1;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService(10);
            _m3205 = _isuService.AddGroup("M3205");
            _nikita = _isuService.AddStudent(_m3205, "Nikita Kurylev");
            _t3200 = _isuService.AddGroup("T3200");
            _ivan = _isuService.AddStudent(_t3200, "Ivan Ivanovich");
            _electiveService = new ElectiveService();
            _securityElective = _electiveService.CreateElective("Cyber security", "N3");
            _securityStream1 = _securityElective.CreateStream(1);
            _biologyElective = _electiveService.CreateElective("Biotechnology", "T3");
            _biologyStream1 = _biologyElective.CreateStream(1);
        }

        [Test]
        public void EnlistDelistTest()
        {
            ElectiveStream securityStream2 = _securityElective.CreateStream(1);
            
            _electiveService.Enlist(_nikita, _securityElective);
            _electiveService.Enlist(_nikita, _securityElective);
            Assert.AreEqual(_securityStream1.Students.First(), _nikita);
            _electiveService.Enlist(_ivan, _securityElective);
            Assert.AreEqual(securityStream2.Students.First(), _ivan);
            _electiveService.Delist(_nikita, _securityElective);
            Assert.AreEqual(_securityStream1.Students.Count, 0);
            _electiveService.Delist(_ivan, _securityElective);
            Assert.AreEqual(securityStream2.Students.Count, 0);
        }

        [TestCase(12, 00, 13, 30)]
        [TestCase(12, 00, 13, 40)]
        [TestCase(12, 30, 12, 00)]
        public void TimeConflict(int hour1, int minute1, int hour2, int minute2)
        {
            var dateTime1 = new DateTime(2021, 10, 12, hour1, minute1, 0);
            var period1 = new Period(dateTime1, new Professor(""), 228);
            _securityStream1.AddClass(period1);
            
            var dateTime2 = new DateTime(2021, 10, 12, hour2, minute2, 0);
            var period2 = new Period(dateTime2, new Professor(""), 137);
            _biologyStream1.AddClass(period2);

            var hourAndHalf = TimeSpan.FromHours(1.5);

            _electiveService.Enlist(_nikita, _securityElective);
            
            Assert.AreEqual(_electiveService.Enlist(_nikita, _biologyElective), dateTime1 >= dateTime2 + hourAndHalf || dateTime2 >= dateTime1 + hourAndHalf);
        }

        [Test]
        public void GetUnlisted()
        {
            _electiveService.Enlist(_isuService.AddStudent(_m3205, "Maxim Rakov"), _biologyElective);
            Assert.AreEqual(_electiveService.GetUnlisted(_m3205)[0], _nikita);
        }
    }
}