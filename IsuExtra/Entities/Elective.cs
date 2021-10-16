using System.Collections.Generic;
using System.Linq;
using Isu.Entities;

namespace IsuExtra.Entities
{
    public class Elective
    {
        private readonly List<ElectiveStream> _electiveStreams;
        private readonly string _faculty;

        public Elective(string name, string faculty)
        {
            Name = name;
            _faculty = faculty;
            _electiveStreams = new List<ElectiveStream>();
        }

        public List<ElectiveStream> Streams => _electiveStreams;
        public string Name { get; }

        public ElectiveStream CreateStream(int capacity)
        {
            var electiveStream = new ElectiveStream(capacity);
            _electiveStreams.Add(electiveStream);
            return electiveStream;
        }

        public bool Enlist(Student student, ElectiveStream electiveStream)
        {
            if (student.Group.Name.StartsWith(_faculty))
                return false;
            if (!_electiveStreams.Contains(electiveStream))
                return false;
            if (Contains(student))
                return false;
            return electiveStream.Enlist(student);
        }

        public bool Delist(Student student)
        {
            ElectiveStream electiveStream = GetStudentStream(student);
            if (electiveStream == null)
                return false;
            return electiveStream.Delist(student);
        }

        public List<ElectiveStream> GetVacantStreams()
        {
            return _electiveStreams.FindAll(e => e.IsVacant());
        }

        public bool Contains(Student student)
        {
            return _electiveStreams.Any(e => e.Contains(student));
        }

        public ElectiveStream GetStudentStream(Student student)
        {
            return _electiveStreams.Find(e => e.Contains(student));
        }
    }
}