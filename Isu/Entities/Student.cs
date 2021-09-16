namespace Isu.Entities
{
    public class Student
    {
        public Student(string name, Group group)
        {
            Name = name;
            Group = group;
        }

        public string Name { get; }

        public Group Group { get; set; }

        public bool CanTransfer(Group group)
        {
            return group.CourseNumber == Group.CourseNumber;
        }
    }
}