using System.Collections.Generic;
using System.Linq;
using Isu.Entities;

namespace IsuExtra.Entities
{
    public class ElectiveStream
    {
        private readonly int _capacity;
        private readonly HashSet<Period> _periods;

        public ElectiveStream(int capacity)
        {
            _capacity = capacity;
            _periods = new HashSet<Period>();
            Students = new HashSet<Student>();
        }

        public HashSet<Student> Students { get; }

        public bool AddClass(Period period)
        {
            return _periods.Add(period);
        }

        public bool Enlist(Student student)
        {
            if (Students.Count >= _capacity)
                return false;
            return Students.Add(student);
        }

        public bool Delist(Student student)
        {
            return Students.Remove(student);
        }

        public bool DoesConflict(Period period)
        {
            return _periods.Any(period.DoesIntersect);
        }

        public bool DoesConflict(ElectiveStream electiveStream)
        {
            return _periods.Any(electiveStream.DoesConflict);
        }

        public bool DoesConflict(IEnumerable<ElectiveStream> electiveStreams)
        {
            return electiveStreams.Any(DoesConflict);
        }

        public bool Contains(Student student)
        {
            return Students.Contains(student);
        }

        public bool IsVacant()
        {
            return Students.Count < _capacity;
        }
    }
}