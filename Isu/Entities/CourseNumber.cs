using Isu.Tools;

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
            set
            {
                if (value < 1 || value > 4)
                {
                    throw new IsuException(
                        $"{nameof(value)} must be between 0 and 4.");
                }

                _number = value;
            }
        }
    }
}