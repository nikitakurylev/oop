using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Isu.Entities;
using IsuExtra.Entities;

namespace IsuExtra.Services
{
    public class ElectiveService
    {
        private readonly List<Elective> _electives;

        public ElectiveService()
        {
            _electives = new List<Elective>();
        }

        public Elective CreateElective(string name, string faculty)
        {
            var elective = new Elective(name, faculty);
            _electives.Add(elective);
            return elective;
        }

        public bool Enlist(Student student, Elective elective)
        {
            if (!_electives.Contains(elective))
                return false;
            Collection<ElectiveStream> studentStreams = GetStudentStreams(student);
            ElectiveStream electiveStream = elective.GetVacantStreams().Find(e => !e.DoesConflict(studentStreams));
            if (electiveStream == null)
                return false;
            return elective.Enlist(student, electiveStream);
        }

        public bool Delist(Student student, Elective elective)
        {
            if (!_electives.Contains(elective))
                return false;
            return elective.Delist(student);
        }

        public List<Student> GetUnlisted(Group group)
        {
            return group.Students.Values.ToList().FindAll(s => GetStudentStreams(s).Count == 0);
        }

        private Collection<ElectiveStream> GetStudentStreams(Student student)
        {
            var electiveStreams = new Collection<ElectiveStream>();
            foreach (Elective elective in _electives.FindAll(e => e.Contains(student)))
                electiveStreams.Add(elective.GetStudentStream(student));

            return electiveStreams;
        }
    }
}