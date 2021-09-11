using System.Collections.Generic;
using Isu.Entities;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private readonly List<Group> _groups;
        private readonly int _maxStudentsPerGroup;
        private int _lastId;

        public IsuService(int maxStudentsPerGroup)
        {
            _groups = new List<Group>();
            _lastId = 0;
            _maxStudentsPerGroup = maxStudentsPerGroup;
        }

        public Group AddGroup(string name)
        {
            var group = new Group(name, _maxStudentsPerGroup);
            _groups.Add(group);
            return group;
        }

        public Student AddStudent(Group group, string name)
        {
            if (FindStudent(name) != null)
                throw new IsuException("Student with such name already exists!");
            _lastId++;
            return group.AddStudent(new Student(name, group), _lastId);
        }

        public Student GetStudent(int id)
        {
            foreach (Group group in _groups)
            {
                if (group.StudentExists(id))
                    return group.GetStudent(id);
            }

            return null;
        }

        public Student FindStudent(string name)
        {
            foreach (Group group in _groups)
            {
                Student student = group.FindStudent(name);
                if (student != null)
                    return student;
            }

            return null;
        }

        public List<Student> FindStudents(string groupName)
        {
            var result = new List<Student>();
            result.AddRange(FindGroup(groupName).Students.Values);
            return result;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var result = new List<Student>();
            foreach (Group group in FindGroups(courseNumber))
            {
                result.AddRange(group.Students.Values);
            }

            return result;
        }

        public Group FindGroup(string groupName)
        {
            return _groups.Find(g => g.Name == groupName);
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _groups.FindAll(g => g.CourseNumber == courseNumber);
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (student.Group.CourseNumber != newGroup.CourseNumber)
                throw new IsuException("Can't transfer between courses!");
            int id = student.Group.RemoveStudent(student);
            newGroup.AddStudent(student, id);
        }
    }
}