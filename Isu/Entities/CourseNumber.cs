namespace Isu.Entities
{
    public class CourseNumber
    {
        private int _number;

        public CourseNumber(int number)
        {
            Number = number;
        }

        private int Number
        {
            get => _number;
            set
            {
                if ((value > 0) && (value < 5))
                {
                    _number = value;
                }
            }
        }
    }
}