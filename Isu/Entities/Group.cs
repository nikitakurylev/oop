using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu.Entities
{
    public class Group
    {
        private readonly Dictionary<int, Student> _students;
        private readonly int _maxStudents;

        public Group(string name, int maxStudents)
        {
            Name = name;
            if (name.Substring(0, 2) != "M3")
                throw new IsuException("Invalid group name!");
            _students = new Dictionary<int, Student>();
            if (!int.TryParse(Name[1..], out int courseNumber))
                throw new IsuException("Invalid group name!");
            CourseNumber = new CourseNumber(courseNumber / 100);
            _maxStudents = maxStudents;
        }

        public string Name { get; }

        public Dictionary<int, Student> Students => _students;

        public CourseNumber CourseNumber { get; set; }

        public Student AddStudent(Student student, int id)
        {
            if (_students.ContainsValue(student))
                throw new IsuException("This student is already in this group!");
            if (_students.Count >= _maxStudents)
                throw new IsuException("Student count exceeds limit!");
            student.Group = this;
            _students.Add(id, student);
            return student;
        }

        public int RemoveStudent(Student student)
        {
            int id = _students.First(s => s.Value.Name == student.Name).Key;
            _students.Remove(id);
            return id;
        }

        public bool StudentExists(int id)
        {
            return _students.ContainsKey(id);
        }

        public Student GetStudent(int id)
        {
            return _students[id];
        }

        public Student FindStudent(string name)
        {
            return _students.Values.ToList().Find(s => s.Name == name);
        }
    }
}